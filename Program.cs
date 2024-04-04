using System;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace RPA_Alura
{
    class Program
    {
        private readonly ICourseService _courseService;

        public Program(ICourseService courseService)
        {
            _courseService = courseService;
        }

        static void Main(string[] args)
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--ignore-certificate-errors");

            //Aqui ocorre a instanciação do driver ignorando erros de certificado para limpar a visualização dos logs, acessa o site da Alura e realiza a busca pelo termo "Games"

            IWebDriver driver = new ChromeDriver(chromeOptions);

            driver.Navigate().GoToUrl("https://www.alura.com.br/");

            if (!IsElementPresent(driver, By.Id("header-barraBusca-form-campoBusca")))
            {
                Console.WriteLine("Campo de busca não encontrado.");
                return;
            }

            IWebElement searchField = driver.FindElement(By.Id("header-barraBusca-form-campoBusca"));
            searchField.SendKeys("Games");
            searchField.SendKeys(Keys.Enter); 

            driver.FindElement(By.ClassName("show-filter-options")).Click();

            if (!IsElementPresent(driver, By.Id("type-filter--0")))
            {
                Console.WriteLine("Opções de filtros não encontrado.");
                return;
            }

            //Aqui ocorre a realização do filtro para somente "Cursos" visto que a plataforma também conta com Podcasts, Artigos, etc 
            //que não se encaixam com as propriedades de um curso

            IWebElement courseFilter = driver.FindElement(By.Id("type-filter--0"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].removeAttribute('hidden');", courseFilter);
            courseFilter.Click();

            driver.FindElement(By.Id("filtros-busca--close")).Click();

            driver.FindElement(By.XPath("//*[@id='busca-form']/form/input[3]")).Click();

            var courseElements = driver.FindElements(By.ClassName("busca-resultado"));

            var dbContext = new AppDbContext();
            var courseService = new CourseService(dbContext);
            var program = new Program(courseService);
            try
            {
                // Nesse loop irá pegar cada informação necessária para armazenamento no database com as devidas validações de erro
                
                foreach (var courseElement in courseElements)
                {
                    if (!IsElementPresent(driver, By.ClassName("busca-resultado-link")))
                    {
                        Console.WriteLine("Link do curso não encontrado.");
                        return;
                    }

                    var courseLink = courseElement.FindElement(By.ClassName("busca-resultado-link"));
                    string courseLinkString = courseLink.GetAttribute("href");

                    if (!IsElementPresent(driver, By.ClassName("busca-resultado-nome")))
                    {
                        Console.WriteLine("Nome do curso não encontrado.");
                        return;
                    }

                    IWebElement titleElement = courseElement.FindElement(By.ClassName("busca-resultado-nome"));
                    string title = titleElement.Text;

                    if (!IsElementPresent(driver, By.ClassName("busca-resultado-descricao")))
                    {
                        Console.WriteLine("Descrição do curso não encontrada.");
                        return;
                    }

                    IWebElement descriptionElement = courseElement.FindElement(By.ClassName("busca-resultado-descricao"));
                    string description = descriptionElement.Text;

                    ((IJavaScriptExecutor)driver).ExecuteScript("window.open();");
                    driver.SwitchTo().Window(driver.WindowHandles.Last());
                    driver.Navigate().GoToUrl(courseLinkString);

                    if (!IsElementPresent(driver, By.CssSelector("p[class='courseInfo-card-wrapper-infos']")))
                    {
                        Console.WriteLine("Duração do curso não encontrada.");
                        return;
                    }
                    
                    IWebElement durationElement = driver.FindElement(By.CssSelector("p[class='courseInfo-card-wrapper-infos']"));
                    string duration = durationElement.Text;
                    string durationNumber = duration.Substring(0, duration.Length - 1);

                    string professor;

                    try
                    {
                        IWebElement professorElement = driver.FindElement(By.ClassName("instructor-title--name"));
                        professor = professorElement.Text;
                    }
                    catch (NoSuchElementException)
                    {
                        professor = "Alura";
                    }

                    driver.Close();
                    driver.SwitchTo().Window(driver.WindowHandles.First());

                    Thread.Sleep(1000);

                    program._courseService.SaveCourse(new Course {Title = title, Description = description, Duration = durationNumber, Professor = professor});
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro no processamento de um curso: {ex.Message}");
            }

            driver.Quit();
        }

        private static bool IsElementPresent(IWebDriver driver, By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}
