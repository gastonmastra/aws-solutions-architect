Import-Module AWS.Tools.S3

$bucketName = Read-Host -Prompt 'Enter bucket name'
$region = "eu-central-1"

Write-Host "AWS Region: $region"
Write-Host "Bucket NameL $bucketName"

Remove-S3Bucket -BucketName $bucketName -Region $region