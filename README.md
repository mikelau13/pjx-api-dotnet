# pjx-api-dotnet

This repository is a group of .NET Core 3.1 dotnet core projects:

- [Pjx_Api](https://github.com/mikelau13/pjx-api-dotnet/blob/master/src/Pjx_Api) - API gateway for business logics (content still TBD), authenticated by [pjx-sso-identityserver](https://github.com/mikelau13/pjx-sso-identityserver).


- [Pjx_CreateCertificates](Pjx_CreateCertificates) - generate self-seigned certificates

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

## Pjx_CreateCertificates

I used this tool to generate self-seigned certificates for the Api and other pjx projects such that [pjx-sso-identityserver](https://github.com/mikelau13/pjx-sso-identityserver)
