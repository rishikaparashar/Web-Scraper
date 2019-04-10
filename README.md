# webscrapercs
Webscraper written in C#

## Prerequisites

### 1. Install .Net 2.1

    1. Download and install .NET core SDK from [https://www.microsoft.com/net/download]

    2. Check .Net version by executing > dotnet --version

### 2. MS SQL Server 2017 (for running in local environment)

    1. Install MS SQL Server Express Edition 2017 from this link : https://www.microsoft.com/en-us/sql-server/sql-server-editions-express
    
### 3. VS Code 1.29

    1. Install VSCode from this link : https://code.visualstudio.com/

### 4. Git

    1. Install git from this link : https://git-scm.com/downloads
    
## Steps to install and run on Azure/Local.

    1. Clone this repo into a local folder using the following command : > git clone https://github.com/dc297/webscrapercs

    2. A new folder named webscrapercs would be created.

    3. To create your own database, run the SQL script - "master/scripts/Script.sql" which will create the database and the tables. 

    4. Go to webscrpercs folder in your local and create a copy of Sample.config in the same directory and name it App.config.

    5. Update the details in App.config (server name, user id and password)of your MS SQL Server instance.
    
    6. To add more Zipcodes to the queue, add them in Zipcode.dat.
    
    7. Execute 'dotnet run' command to run the Webscraper.

    8. To query the data being populated use the scripts in scripts/data.sql. You may replace the property id with your own value.
    
    9. To open the project in VS Code follow these steps :

        1. Open the newly created folder in VS Code. There would be an option under file menu.

        2. VS Code would automatically tell you that there are unresolved dependencies for the project and it'll ask you to add/restore them. Add/Restore those dependencies.
        
        3. Open a new terminal in VS Code from the terminal menu and execute > dotnet run
        
        4. You're good to go !! You should be able to see the parser logs in the terminal. And new entries would be added in the database.

