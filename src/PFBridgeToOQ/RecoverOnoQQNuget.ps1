cd ..
cd ..
if(Test-Path -Path "IBoxsForOnoQQ"){
	Remove-Item "IBoxsForOnoQQ" -Force -Recurse
}
git clone https://github.com/zqu1016/IBoxsForOnoQQ.git
Copy-Item -Path "IBoxsForOnoQQ\packages" -Destination "src\" -Force -Recurse
if(Test-Path -Path "IBoxsForOnoQQ"){
	Remove-Item "IBoxsForOnoQQ" -Force -Recurse
}
cd src
ls
cd packages
ls
