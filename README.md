# HappyDarioBot
Get Happy birthday greetings from Dario!

# Introduction
Dario is funny 😊. He gives great happy birthday greetings! So why don't let him greet everyone who would like to be greeted?

Here it is a Telegram bot that let you receive your personalized happy birthday greetings. Just search for @HappyDarioBot on Telegram and write your name.
You will receive your personalized greetings audio.

# Getting started with repo
HappyDarioBot is implemented as an Azure function.

Integration and acceptance tests uses real remote Azure storage and telegram API, so in order to get started with repo, you will need 
 * An Azure account
 * An Azure storage
 * A Telegram bot (https://core.telegram.org/bots#botfather)
 
Tests reads azure connection string and bot token from **local.settings.json**. 
Edit **local.settings.example.json** and then rename it to local.settings.json. Use the following entries:

 * **BotToken**: is the Telegram bot token
 * **ResourcesPath**: Your local folder path when HappyDarioBot.LocalFileRepository is used
 * **RemoteResourcesPath**: The remote path on azure of the directory where audio files are stored (path is in the form **your/path**)
 * **AzureStorageName**: the name of both azure file and blob storages
 * **ForwardToId**: This is the id of the Telegram user that receive request to register an Audio when it is missing
 * **LogToId**: This is the id of the Telegram user that monitors the bot
 * **WEBSITE_CONTENTAZUREFILECONNECTIONSTRING**: the connection string of the Azure storage account

# Deploy on Azure
If you want to deploy the bot, follow deploy instructions on azure tutorials or inside Visual Studio. 
Then you have to create all of the local.setting.json keys in the application configuration of the azure function:

 - click on configuration <img src=https://github.com/lucapiccinelli/HappyDarioBot/blob/master/Docs/assets/configuration.png width=1000px />
 - edit the entries <img src=https://github.com/lucapiccinelli/HappyDarioBot/blob/master/Docs/assets/configuration2.png width=1000px />
