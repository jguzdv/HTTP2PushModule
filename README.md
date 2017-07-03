
# HTTP2PushModule
IIS Module to create HTTP2 Push Promises.  
It allows to define "PushGroups" which will be provided as push promise, if specific files of the PushGroups are requested.
Be aware that most browsers will need SSL to use HTTP2. 

## How to install the Module
Just copy the Module to the Bin Path of an Application, where you want to use the Push Module.

## How to configure the Module

Configuration by example:

```XML
<configuration>
  <configSections>
    <section name="Http2PushGroups" type="ZDV.HTTP2PushModule.Configuration.PushGroupSection" />
  </configSections>
  
  <system.webServer>
    <!-- add the module -->
    <modules runAllManagedModulesForAllRequests="true">
      <add name="Http2PushModule" type="ZDV.HTTP2PushModule.Http2PushModule" />
    </modules>
  </system.webServer>

  <Http2PushGroups>
    <PushGroup name="Example1">
      <PushElement url="~/main.html" triggers="true" />
      <PushElement url="~/app.js" />
      <PushElement url="~/app.css" />
    </PushGroup>
    <PushGroup name="Example2">
      <PushElement url="~/app.js" triggers="true" />
      <PushElement url="~/require.js" />
      <PushElement url="~/modules/module1.js" />
      <PushElement url="~/modules/module2.js" />
    </PushGroup>
  </Http2PushGroups>  
</configuration>

```
