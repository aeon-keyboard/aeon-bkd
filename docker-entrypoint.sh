#!/bin/bash
set -e

/app/check-secrets.sh

exec dotnet aeon-bkd.dll