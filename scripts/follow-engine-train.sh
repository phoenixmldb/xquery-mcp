#!/usr/bin/env bash
# Moves this repo's engine pin to the latest COMPLETED engine train, for follow-engine-train.yml.
#
# Why. This MCP server embeds the engine, and its version IS the engine train's version
# (check-release-train.sh refuses a tag that differs from the pin). It sat on the 1.7.0 engine
# from 2026-09-11 until 2026-10-01 because releasing it was a step nobody's checklist had, so
# users of the MCP server ran an engine five releases old, missing the 2.5 security fixes.
#
# "Completed" means the train's last step has published: the xquery4 CLI (cli-v<version>) goes
# out after both libraries. Following the library alone would release mid-train: XQuery 2.5.0
# was on nuget.org for hours before Xslt 2.5.1 existed.
#
# Reads nuget.org, not git tags, so it follows what a consumer can actually install.
# Prints "version=<v>" and "changed=true|false" for $GITHUB_OUTPUT; exits non-zero only on error.
set -euo pipefail

engine="${1:?usage: follow-engine-train.sh <engine package id> [props]}"
props="${2:-Directory.Packages.props}"

stable_versions() { # package id -> its stable versions, ascending
  curl -fsS "https://api.nuget.org/v3-flatcontainer/$(printf '%s' "$1" | tr '[:upper:]' '[:lower:]')/index.json" \
    | python3 -c 'import sys,json; print("\n".join(v for v in json.load(sys.stdin)["versions"] if v.replace(".","").isdigit()))' \
    | sort -V
}

train="$(stable_versions xquery4 | tail -1)"
[ -n "$train" ] || { echo "follow-engine-train: could not read the xquery4 versions from nuget.org" >&2; exit 2; }
pin="$(grep -oP "(?<=Include=\"$engine\" Version=\")[^\"]+" "$props" || true)"
[ -n "$pin" ] || { echo "follow-engine-train: no $engine pin in $props" >&2; exit 2; }
echo "train $train (xquery4 on nuget.org), $engine pinned $pin"

out() { echo "version=$train"; echo "changed=$1"; }
if [ "$pin" = "$train" ]; then echo "up to date"; out false; exit 0; fi
if ! stable_versions "$engine" | grep -qx "$train"; then
  echo "$engine $train is not on nuget.org; train $train does not include it"; out false; exit 0
fi
if git rev-parse -q --verify "refs/tags/v$train" >/dev/null; then
  echo "v$train is already tagged here; not re-releasing"; out false; exit 0
fi
if [ "$(printf '%s\n%s\n' "$pin" "$train" | sort -V | tail -1)" != "$train" ]; then
  echo "pin $pin is AHEAD of train $train; leaving it alone"; out false; exit 0
fi

sed -i "s#Include=\"$engine\" Version=\"$pin\"#Include=\"$engine\" Version=\"$train\"#" "$props"
grep -q "Include=\"$engine\" Version=\"$train\"" "$props" || { echo "follow-engine-train: pin edit failed" >&2; exit 2; }
echo "bumped $engine $pin -> $train"
out true
