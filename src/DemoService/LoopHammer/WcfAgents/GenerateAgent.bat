@echo off

SvcUtil.exe  /serializable /syncOnly /ct:System.Collections.Generic.List`1  /n:*,DemoService.WcfAgents http://localhost:8733/Design_Time_Addresses/WcfService/Service1/ /out:WCFAgent.cs

