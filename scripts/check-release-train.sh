#!/usr/bin/env bash
# Refuses to pack a release whose engine pin is not the release version.
#
# Ported from phoenixmldb-xslt, where it exists because published PhoenixmlDb.Xslt 1.6.13
# depends on PhoenixmlDb.XQuery 1.6.12 — the XSLT tag was cut before the XQuery push. Nothing
# was "wrong" at pack time, since the pin was the latest published version, so a staleness
# check cannot catch it. Only an equality check can:
#
#   if this train is 1.8.0, every train-locked pin must read 1.8.0.
#
# Why this repo needs it. `xquery-mcp` ships an MCP server that EMBEDS the engine: a user who
# installs xquery-mcp 1.8.0 is running whatever PhoenixmlDb.XQuery this pin names. Without an
# equality check the tool and the engine drift silently, which is exactly what happened — this
# repo sat pinned at 1.6.14 through the entire 1.7.0 train and nothing said so. check-pins.sh
# catches a pin that has fallen behind what is PUBLISHED; only this catches a pin that has
# fallen behind the train being released.
#
# Mark a pin with "check-pins: train-locked" in the comment above it to opt in. Packages on
# their own cadence are deliberately not locked.
set -uo pipefail

version="${1:-}"
props="${2:-Directory.Packages.props}"
[ -n "$version" ] || { echo "usage: check-release-train.sh <release-version> [props]"; exit 2; }
version="${version#v}"
[ -f "$props" ] || { echo "check-release-train: no $props here"; exit 2; }

fail=0 locked=0
while read -r id ver; do
  if ! grep -B8 "Include=\"$id\"" "$props" | grep -q 'check-pins: train-locked'; then
    echo "free  $id $ver (not train-locked)"
    continue
  fi
  locked=$((locked+1))
  if [ "$ver" = "$version" ]; then
    echo "ok    $id $ver == train $version"
  else
    echo "FAIL  $id is pinned $ver but this train is $version"
    echo "      Packing now ships a server whose engine is not the one this release tested."
    fail=1
  fi
done < <(grep -oE '<PackageVersion Include="(PhoenixmlDb[^"]*)" Version="([^"]+)"' "$props" \
         | sed -E 's/.*Include="([^"]+)" Version="([^"]+)".*/\1 \2/')

# Fails CLOSED: finding no train-locked pins is an error, not a pass. A check that silently
# verifies nothing is worse than no check, because it occupies the place where one would go.
if [ "$locked" -eq 0 ]; then
  echo "check-release-train: no train-locked pins found — mark them with 'check-pins: train-locked'"
  exit 2
fi
exit $fail
