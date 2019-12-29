# Reddit Purge

__Want to get off Reddit?__

Are you done and want to go back to anonymity (or maybe start fresh)?  
Unfortunately, Reddit will not provide you with an easy way to delete your account history, you can only deactivate your account, leaving everything you created untouched.  
You can always delete your posts and comments history manually, but that can take hours, depending on how active you were.  
In addition, Reddit does not have an API to delete your comments history.  

There has been some attempts at this using browser developer tools and such, but it wasn't working for me.  

This app uses Selenium (browser automation tool) and .NET Core (cross platform development platform) to automate Chrome and simulate a user launching a browser, logging in to Reddit and manually deleting his entire history.  
This might take a few hours, and is **completely irreversible**. So, proceed with great caution.

It's also worth noting that the *internet never forgets*, and there are several reddit mirrors/archives that might still contain a copy of your posts.  
**This app will not delete those copies.**


## How to use
1. You can check out the code, and run it from source
2. Download the compiled binaries (windows) from the [Release](https://github.com/odedolive/RedditPurge/releases) tab.

### Steps to Compile From Source
You will need to download and install the .NET Core 3.1 SDK  
After the SDK is downloaded and installed simply navigate to the "src\RedditPurgeApp" folder and type `dotnet run`  
You will be prompted for your Reddit's username and password.  
These credentials are not sent anywhere, and will only be used to authenticate you **locally**
