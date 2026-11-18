#!/usr/bin/env bash
# Usage: check-packed-version.sh <dir with the .nupkg> <version>
#
# Starts the server dll from the packed tool package, sends the MCP initialize request, and
# fails unless serverInfo.version is <version> (or <version>.0). The check runs the packed dll,
# not the build output, because a later build step can write over the build output before pack.
set -euo pipefail

dir=$1
version=$2

pkg=$(find "$dir" -maxdepth 1 -name '*.nupkg' ! -name '*.symbols.nupkg' | head -n 1)
if [ -z "$pkg" ]; then
  echo "No .nupkg in $dir" >&2
  exit 1
fi

work=$(mktemp -d)
trap 'rm -rf "$work"' EXIT
unzip -q "$pkg" -d "$work"

dll=$(find "$work/tools" -name '*-mcp-server.dll' | head -n 1)
if [ -z "$dll" ]; then
  echo "No *-mcp-server.dll in $pkg" >&2
  exit 1
fi

request='{"jsonrpc":"2.0","id":1,"method":"initialize","params":{"protocolVersion":"2025-06-18","capabilities":{},"clientInfo":{"name":"ci","version":"1"}}}'
out=$( { printf '%s\n' "$request"; sleep 15; } | timeout 60 dotnet "$dll" 2>/dev/null || true)

info=$(printf '%s' "$out" | grep -o '"serverInfo":{[^}]*}' | head -n 1 || true)
echo "$(basename "$pkg"): ${info:-no serverInfo in the initialize response}"

reported=$(printf '%s' "$info" | sed -n 's/.*"version":"\([^"]*\)".*/\1/p')
if [ "$reported" != "$version" ] && [ "$reported" != "$version.0" ]; then
  echo "serverInfo.version is '${reported}', expected '${version}'" >&2
  exit 1
fi
