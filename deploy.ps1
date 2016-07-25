
# ----------------------
# KUDU Deployment Script
# Version: 1.0.7
# ----------------------

# Helpers
# -------

function exitWithMessageOnError($1) {
  if ($? -eq $false) {
    echo "An error has occurred during web site deployment."
    echo $1
    exit 1
  }
}

# Prerequisites
# -------------

# Verify node.js installed
where.exe node 2> $null > $null
exitWithMessageOnError "Missing node.js executable, please install node.js, if already installed make sure it can be reached from current environment."

# Setup
# -----

$SCRIPT_DIR = $PSScriptRoot
$ARTIFACTS = "$SCRIPT_DIR\..\artifacts"

$KUDU_SYNC_CMD = $env:KUDU_SYNC_CMD

$DEPLOYMENT_SOURCE = $env:DEPLOYMENT_SOURCE
$DEPLOYMENT_TARGET = $env:DEPLOYMENT_TARGET

$NEXT_MANIFEST_PATH = $env:NEXT_MANIFEST_PATH
$PREVIOUS_MANIFEST_PATH = $env:PREVIOUS_MANIFEST_PATH

if ($DEPLOYMENT_SOURCE -eq $null) {
  $DEPLOYMENT_SOURCE = $SCRIPT_DIR
}

if ($DEPLOYMENT_TARGET -eq $null) {
  $DEPLOYMENT_TARGET = "$ARTIFACTS\wwwroot"
}

if ($NEXT_MANIFEST_PATH -eq $null) {
  $NEXT_MANIFEST_PATH = "$ARTIFACTS\manifest"

  if ($PREVIOUS_MANIFEST_PATH -eq $null) {
    $PREVIOUS_MANIFEST_PATH = $NEXT_MANIFEST_PATH
  }
}

if ($KUDU_SYNC_CMD -eq $null) {
  # Install kudu sync
  echo "Installing Kudu Sync"
  npm install kudusync -g --silent
  exitWithMessageOnError "npm failed"

  # Locally just running "kuduSync" would also work
  $KUDU_SYNC_CMD = "$env:APPDATA\npm\kuduSync.cmd"
}

$DEPLOYMENT_TEMP = $env:DEPLOYMENT_TEMP
$MSBUILD_PATH = $env:MSBUILD_PATH

if ($DEPLOYMENT_TEMP -eq $null) {
  $DEPLOYMENT_TEMP = "$env:temp\___deployTemp$env:random"
  $CLEAN_LOCAL_DEPLOYMENT_TEMP = $true
}

if ($CLEAN_LOCAL_DEPLOYMENT_TEMP -eq $true) {
  if (Test-Path $DEPLOYMENT_TEMP) {
    rd -Path $DEPLOYMENT_TEMP -Recurse -Force
  }
  mkdir "$DEPLOYMENT_TEMP"
}

if ($MSBUILD_PATH -eq $null) {
  $MSBUILD_PATH = "${env:ProgramFiles(x86)}\MSBuild\14.0\Bin\MSBuild.exe"
}
##################################################################################################################################
# Deployment
# ----------

echo "Handling ASP.NET Core Web Application deployment with MSBuild."

# 1. Restore nuget packages
nuget.exe restore "$DEPLOYMENT_SOURCE\AppServiceInfo.sln" -packagesavemode nuspec
exitWithMessageOnError "NuGet restore failed"

# 2. Build and publish
& "$MSBUILD_PATH" "$DEPLOYMENT_SOURCE\AppServiceInfo.sln" /nologo /verbosity:m /p:deployOnBuild=True`;AutoParameterizationWebConfigConnectionStrings=false`;Configuration=Release`;UseSharedCompilation=false`;publishUrl="$DEPLOYMENT_TEMP" $SCM_BUILD_ARGS
exitWithMessageOnError "MSBuild failed"

# 3. KuduSync
& $KUDU_SYNC_CMD -v 50 -f "$DEPLOYMENT_TEMP" -t "$DEPLOYMENT_TARGET" -n "$NEXT_MANIFEST_PATH" -p "$PREVIOUS_MANIFEST_PATH" -i ".git;.hg;.deployment;deploy.ps1"
exitWithMessageOnError "Kudu Sync failed"

##################################################################################################################################
echo "Finished successfully."
