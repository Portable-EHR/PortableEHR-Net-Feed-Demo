# Portable EHR .Net Feed Demo

> The intention of this project is to demonstrate how to interface with Portable EHR FeedHub using the Portable EHR .Net Feed SDK.
>
> Being a demonstration this code is not intended to be use in production, and the following aspects had been ignored:
>
>  - Multithreading
>  - Circuit brakers
>  - Configuration flexibility
>  - Proper error management
>  - Unit testing
>  - UX (I know... I know...)
>  - etc.

### Running the application

If you are using JetBrains Rider you have the configuration to run the project in .run/PortableEHRNetFeedDemo.run.xml
If you are running the project from the command line:
  - cd ${PROJECT_ROOT} // where README.md is
  - (optional) dotnet clean
  - dotnet run

Start using it :
- Go to [Console web](https://localhost:4004) to :
    - Change which answer to use for each server endpoint
    - See a live history of request received by the Feed server
    - Sent requests to FeedHub
    - See the responses of FeedHub
- By deleting, adding, and editing the json files under ${PROJECT_ROOT}/mocks you can customize the requests templates and the responses sent (stop and re-run the application to see the changes)
- Import `Feed.postman_collection.json` and `Feed_local.postman_environment.json` in [Postman](https://www.postman.com/) and start hitting your brand-new Feed

Notice :
- Some of the response's id will be changed (to create new appointments for example)
- The lastUpdated attribute of the objects will be updated too
- The appointments startTime and endTime will be set to 48 and 49 hours from the time when the request is received

### You want to integrate your project now?
This projects uses the [Portable EHR .Net Feed SDK](https://github.com/Portable-EHR/PortableEHR-Feed-Net-SDK). Install the NuGet package in your project to have a jump start
```xml
<ItemGroup>
  ...
  <PackageReference Include="PortableEHR.Feed.SDK" Version="1.0.0" />
</ItemGroup>
```

### References
- [The swagger to Feed API this project (and any Feed) IMPLEMENTS](https://feed.portableehr.io:4004/docs)
- [The swagger to FeedHub API this project (and any Feed) CONSUMES](https://feed.portableehr.io:3004/docs)
