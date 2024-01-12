//+ realizada importação de dependencias
//dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson
//dotnet add package RestSharp

//+ criada classe base de Dados de Acesso, com url, login e senha.
//Model/DadosDeAcesso.cs
//+ criada classe extensão para leitura dos dados de acesso
// Model/Extensions/DadosDeAcessoExtension.cs
// criado arquivo appsettings.json com as configurações de acesso (url base, usuário e senha) e função para ler dele
// criada variavel de ambiente no PC, MeuAcesso, com os dados de acesso: https://localhost:7169/api/v1; alexandre; admin123 (sendo baseUrl; User; password)  e função para ler dele

//+ Criada Pasta Requests onde vão ficar as classes de solicitações a api
// criada classe Token, em que ela recebe os dados de acesso e é responsavel por fornecer os token (depois de instanciada, ela permite acesso ao token e refreshtoken)

//+ Criada classe Person, para receber os dados da API
// Model/Person
//! criada pasta Link e baixado o package dotnet add package RestSharp.JsonNetCore para poder ler os links enviados (senão ou removo os links de person, ou os dados vem em branco)

//+ Criada em Requests a classe para recebimento e envio de Persons



//*inicio do programa
using ExemploDeConsumoDaRestAPIviaSSharp.Model;
using ExemploDeConsumoDaRestAPIviaSSharp.Model.Extension;
using ExemploDeConsumoDaRestAPIviaSSharp.Requests;

DadosDeAcesso dadoDeAcesso = new DadosDeAcesso();
dadoDeAcesso.DadosDeJson();
//dadoDeAcesso.DadosDeVariavelDeAmbiente("MeuAcesso");

Token token = new Token(dadoDeAcesso);

//+buscando uma pessoa
PersonsAPI personAPI = new PersonsAPI(token);
Person person = personAPI.buscaPorId(1);
Console.WriteLine($"Busca por Id: {person.Nome}");

//+ buscando todas
foreach (Person personFromList in personAPI.buscaTodos())
{
    Console.WriteLine($"Pessoa id {personFromList.Codigo}: {personFromList.Nome}");
}

//+inserindo pessoa
Person newPerson = new Person
{
    Nome = "Inserido via cliente",
    Endereco = "Inserido via cliente",
    Enabled = true,
    Sobrenome = "Inserido via cliente",
    Gender = "masc"
};

newPerson = personAPI.insereNovo(newPerson);
Console.WriteLine($"Inserido novo: {newPerson.Nome}");

//+ atualizando pessoa
Console.WriteLine($"Nome antigo: {newPerson.Nome}");
newPerson.Nome = "Meu novo nome";
personAPI.atualizaPerson(newPerson);
Console.WriteLine($"Nome Novo: {personAPI.buscaPorId(newPerson.Codigo).Nome}");

//+deletando pessoa
var idMaisAlto = personAPI.buscaTodos().Max(p => p.Codigo);
if (personAPI.deletaPerson(idMaisAlto))
{
    Console.WriteLine($"Deletada pessoa {idMaisAlto}");
}
else
{
    Console.WriteLine($"Falha ao deletar a pessoa {idMaisAlto}");
};
