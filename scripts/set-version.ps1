$version = $(git describe --tags | %{$_ -replace '-([^g])', '.$1'})
Update-AppveyorBuild -Version $version
