To get US-Plus data from elastic:

Install elastic dump via npm:

```cmd
npm i -g elasticdump
```

Dump the data. Use **cmd** - not powershell!

```cmd
elasticdump --input=https://<username>:<password>@040e422d.qb0x.com:30185/propertylistings-l --output=data.json --searchBody {\"query\":{\"wildcard\":{\"Common.PrimaryKey\":\"us-plus*\"}}}
```

Note - us-plus must be lowercase - wildcard doesn't work with upper case?
