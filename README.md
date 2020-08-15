# pjx-api-dotnet

This repository is a group of .NET Core 3.1 dotnet core projects:

- [Pjx_Api](https://github.com/mikelau13/pjx-api-dotnet/blob/master/src/Pjx_Api) - API gateway for business logics (content still to be developed), authenticated by [pjx-sso-identityserver](https://github.com/mikelau13/pjx-sso-identityserver).
- [Pjx_CreateCertificates](https://github.com/mikelau13/pjx-api-dotnet/blob/master/src/Pjx_CreateCertificates) - generate self-seigned certificates
- [Pjx_CreateCertificates_Test](https://github.com/mikelau13/pjx-api-dotnet/blob/master/src/Pjx_CreateCertificates_Test) - TDD of the Pjx_CreateCertificates

This project is one of the components of the `pjx` application, please check [pjx-root](https://github.com/mikelau13/pjx-root) for more details.

## Pjx_Api

A simple API project developed with .Net Core 3.1, being consumed by [pjx-web-react](https://github.com/mikelau13/pjx-web-react).


### Dependencies

[pjx-sso-identityserver](https://github.com/mikelau13/pjx-sso-identityserver) - to handle the Api's authentications.

> Please follow the instructions in [pjx-sso-identityserver](https://github.com/mikelau13/pjx-sso-identityserver) to set up the `hosts` and trust the self-signed SSL certificate, otherwise this API might refuse to authenticate.


### To run

Run this command:

```bash
docker-compose up
```

Or run it with dotnet for local debugging:

```bash
dotnet build && dotnet run
```

### To use with Swagger

Then you can visit the swagger UI: http://localhost:6001/swagger/ or the swagger specification: http://localhost:6001/swagger/v1/swagger.json

You will need the token to execute most of the APIs in this project, you can obtain a token from [pjx-sso-identityserver](https://github.com/mikelau13/pjx-sso-identityserver), please follow the steps to register in [pjx-root](https://github.com/mikelau13/pjx-root#using-the-web-app), login to the web page, then visit the `/country/all` page and get the key from the browser's developer tool.

![pjx get token](/images/api_swagger_key.png)

Then in the swagger page, click the `authorize` button, paste the token `Bearer xxxxxx`:

![pjx paste token](/images/api_swagger_authorize.png)

Then it should return successful response 200:

![pjx response 200](/images/api_swagger_response_200.png)


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
