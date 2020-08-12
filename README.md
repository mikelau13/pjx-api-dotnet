# pjx-api-dotnet

This repository is a group of .NET Core 3.1 dotnet core projects:

- [Pjx_Api](https://github.com/mikelau13/pjx-api-dotnet/blob/master/src/Pjx_Api) - API gateway for business logics (content still TBD), authenticated by [pjx-sso-identityserver](https://github.com/mikelau13/pjx-sso-identityserver).


- [Pjx_CreateCertificates](https://github.com/mikelau13/pjx-api-dotnet/blob/master/src/Pjx_CreateCertificates) - generate self-seigned certificates

- [Pjx_CreateCertificates_Test](https://github.com/mikelau13/pjx-api-dotnet/blob/master/src/Pjx_CreateCertificates_Test) - TDD of the Pjx_CreateCertificates


## Pjx_Api

A simple Api project developed with .Net Core 3.1, being consumed by [pjx-web-react](https://github.com/mikelau13/pjx-web-react).


### Dependencies

[pjx-sso-identityserver](https://github.com/mikelau13/pjx-sso-identityserver) - to handle the Api's authentications.

> **To do**: there are issues to consume the `pjx-sso-identityserver` now because I do not have a real DNS name for my application, will need to implement workaround such that configure `hosts` local file, allowing self-signed SSL certificate on web browsers, etc.


### To Use

Run this command:

```bash
docker-compose up
```

Or run it with dotnet for local debugging:

```bash
dotnet build && dotnet run
```

## To generate self-signed certificate for Identity Server

Execute the follow PowerShell script (but replace openssl path and output paths):

```
$certPass = "password"
$certSubj = "host.docker.internal"
$certAltNames = "DNS:localhost,DNS:pjx-sso-identityserver,DNS:host.docker.internal,DNS:127.0.0.1"
$opensslPath="C:\Program Files\Git\usr\bin"
$workDir="D:\Codes"
$dockerDir=Join-Path $workDir "ProjectApi"
Start-Process -NoNewWindow -Wait -FilePath (Join-Path $opensslPath "openssl.exe") -ArgumentList "req -x509 -nodes -days 365 -newkey rsa:2048 -keyout ",
                                          (Join-Path $workDir pjx-sso-identityserver.rsa_2048.cert.key),
                                          "-out", (Join-Path $dockerDir pjx-sso-identityserver.rsa_2048.cert.crt),
                                          "-subj `"/CN=$certSubj`" -addext `"subjectAltName=$certAltNames`""
										  
Start-Process -NoNewWindow -Wait -FilePath (Join-Path $opensslPath "openssl.exe") -ArgumentList "pkcs12 -export -in ", 
                                           (Join-Path $dockerDir pjx-sso-identityserver.rsa_2048.cert.crt),
                                           "-inkey ", (Join-Path $workDir pjx-sso-identityserver.rsa_2048.cert.key),
                                           "-out ", (Join-Path $workDir pjx-sso-identityserver.rsa_2048.cert.pfx),
                                           "-passout pass:$certPass"							   
										   
#this will prompt for the password
$cert = Get-PfxCertificate -FilePath (Join-Path $workDir "pjx-sso-identityserver.rsa_2048.cert.pfx") 
```
