$files = Get-ChildItem .\Resources\* -Include *.exe, *.dll

foreach($file in $files) {
	signtool sign /n "ZSA Technology Labs Inc." /t http://time.certum.pl/ /fd sha256 /v $file
}
