Import-Module AWS.Tools.S3

$region = "eu-central-1"
$bucketName = Read-Host -Prompt 'Enter bucket name'

Write-Host "AWS Region: $region"
Write-Host "Bucket Name: $bucketName"


New-S3Bucket -BucketName $bucketName -Region $region