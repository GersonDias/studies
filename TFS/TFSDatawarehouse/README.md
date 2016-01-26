# TFS Data warehouse Custom Adapter
## TFS Data warehouse Custom Adapter Sample

**What that will do:**
This project will collect how many and who is the people and what software they are using to connect to Team Foundation Server. This adapter collect the data from tbl_command table and save this on a new database called 'TFSCustomWarehouse' so this data won't suffer by Warehouse Rebuild and we can query by this information after 14 days, the default time to tbl_command delete their data.


**How to build this project:**
The Team Foundation Server's licence don't allow me to distribute this ones. So, you don't will find an Nuget package for them and you can't find them on TFS Object Model too. So, the best way to get the needed's DLLs is installing the [TFS Express Edition](http://visualstudio.com/download) or picking them in a TFS Server.

**Little _tricks_:**
The projects with name like `*2012` and `*2013` has exactly the same source code. The difference between both are the TFS binaries version. So, I used a little trick: I make a folder called 'Shared' and inside I put the .cs files. In the projects, I add this files as links for the files on 'Shared' folder. In this way, we don't need to bother about changes on 2012 version and worry about make this same change in 2013 version. This kind of solution is great when you need to share the same source code with different product binaries versions. Take a look in the new Universal Apps and you will see a project called 'Shared Project'... we doing something like that here.

------

# TFS Data Warehouse
## Exemplo de Customização do TFS Datawarehouse

**O que esse projeto faz:**
Este Adaptador customizado irá coletar informações sobre quem, quantas vezes e com que software acessou o TFS. Ele irá coletar dados da tabela tbl_command e salvará em um novo Banco de Dados para que as informações nela contida não seja apagada pelo comando Rebuild Datawarehouse do TFS. Com isto, poderá-se fazer queries nesta tabela de informações com espaço de tempo maior que 14 dias, valor padrão para que os dados da tabela tbl_command sejam apagados.

**Como compilar o projeto:**
Dll's de acesso ao Team Foudation Server (TFS) são proprietárias e não podem ser re-distribuidas. Por este motivo, você não vai encontrar um pacote Nuget com estas dependencias, e algumas delas não podem ser encontradas nem mesmo instalando o TFS 2013 Object Model. Assim, existem duas formas de obter estas Dll's: Instalar o TFS Express em sua máquina de desenvolvimento ou obter estes binários de um servidor do TFS.

**Pequenos truques:**
Os projetos com sufixo 2012 e 2013 tem exatamente o mesmo código. A diferença entre as duas são as versões das Dll's do TFS referenciadas. Portanto, resolvi usar um pequeno truque dentro da solution: Criei uma “solution folder” chamada Shared e dentro dela coloquei os arquivos .cs. Dentro de cada projeto, adiciono estes arquivos como um “link” para o arquivo físico. Assim, não preciso me preocupar em fazer alguma mudança na versão 2012 que tenha que ser refeita na versão 2013, ao mesmo tempo que cada projeto em sua pasta bin terá os compilados da mesma maneira e as referências para as versões corretas das Dll's Microsoft.TeamFoundation.*. Este tipo de solução é comum quando temos que trabalhar com diversas versões de Dll's e o mesmo código fonte, tanto que os novos Shared Projects funcionam basicamente da mesma forma, porém deixando tudo “visualmente mais elegante”.
