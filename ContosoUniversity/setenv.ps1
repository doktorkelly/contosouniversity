#

$NUNIT_HOME     = ".\packages\NUnit.Runners.2.6.3\tools"
$OPENCOVER_HOME = ".\packages\OpenCover.4.5.2316"
$REPORTGEN_HOME = ".\packages\ReportGenerator.1.9.1.0"
$STYLECOP_HOME  = ".\packages\StyleCop.MSBuild.4.7.45.0\tools"

$env:path = $env:path + ";$NUNIT_HOME\bin"
$env:path = $env:path + ";$OPENCOVER_HOME"
$env:path = $env:path + ";$REPORTGEN_HOME"
$env:path = $env:path + ";$STYLECOP_HOME"

$env:path -split ";"
