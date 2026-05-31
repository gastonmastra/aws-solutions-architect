# Initiate a PowerShell session
```bash
pwsh
```

# Install AWSTools Installer
```powershell
Install-Module -Name AWS.Tools.Installer
```

# Install Modules
```powershell
Install-AWSToolsModule AWS.Tools.EC2,AWS.Tools.S3 -CleanUp
```

## Reference
https://docs.aws.amazon.com/powershell/v4/reference/