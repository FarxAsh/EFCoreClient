using EFCoreClient.Data;
using EFCoreClient.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EFCoreClient.Data.Enums;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using Microsoft.Data.SqlClient;

namespace EFCoreClient.Services
{
    public class DbClientConsoleWindowService
    {
        readonly OrderRepository orderRepository;
        private readonly UserRepository userRepository;
        private readonly BookRepository bookRepository;

        public DbClientConsoleWindowService(OrderRepository orderRepository, UserRepository userRepository, BookRepository bookRepository)
        {
            this.orderRepository = orderRepository;
            this.userRepository = userRepository;
            this.bookRepository = bookRepository;
        }

        public async Task RunDbAndClientCommunication()
        {
            User user = new User();
            
            Console.WriteLine("Для того, чтобы остановить сеанс, необходимо ввести \"stop\"");
            
            //Вход в систему
            try
            {
                user = await AuthenticationProccessAsync();
                Console.WriteLine("Приветствуем пользователя: {0} {1}", user.FirstName, user.LastName);
            }
            catch
            {
                throw;
            }
            Console.WriteLine("Ниже представлен весь имеющийся ассортимент книг");
            Console.WriteLine();

            //Формирование заказа
            while (true)
            {
                string inputString = String.Empty;
                int paymentTypeId, paymentStatusId, attemptCount;
                DateTime deleveryDate;
                List<int> bookIds = new List<int>();
                Order createdOrder;


                //Вывод всего ассортимента книг
                try
                {
                    Console.WriteLine("{0,-10} {1,-25} {2,-25} {3,-10} {4,-15}", "Id", "Title", "Publisher", "Price", "In store status");
                    var books = await bookRepository.GetAllBooksAsync();
                    foreach (var book in books)
                    {
                        Console.WriteLine("{0,-10} {1,-25} {2,-25} {3,-10:#,###.#} {4,-15}", book.Id, book.BookTitle, book.PublisherName, book.BookPrice, book.InStoreStatus);
                    }
                }
                catch(Exception ex)
                {
                    throw;
                }
                Console.WriteLine();

                //Считывание id заказываемых книг
                attemptCount = 4;
                while(attemptCount != 0)
                {
                    Console.WriteLine("Ввведите через пробел в одну строчку Id желаемых книг");
                    inputString = Console.ReadLine();
                    if (inputString == "stop")
                    {
                        while (attemptCount != 0)
                        {
                            Console.WriteLine("Вы уверены, что хотите закончить сеанс?[y,n]");
                            inputString = Console.ReadLine();
                            if (inputString == "y") Environment.Exit(0);
                            else if (inputString == "n") break;
                            else
                            {
                                attemptCount--;
                                Console.WriteLine("Вы ввели несуществующую команду");
                                if (attemptCount == 0)
                                {
                                    Console.WriteLine("У вас закончились попытки для входа. Сессия будет принудительно закрыта");
                                    Environment.Exit(0);
                                }
                                Console.WriteLine("Вам будет предоставлена еще одна попытка. Осталось попыток: {0}", attemptCount);
                                continue;
                            }
                        }
                        continue;
                    }
                    else if (String.IsNullOrWhiteSpace(inputString))
                    {
                        attemptCount--;
                        Console.WriteLine("Заказываемые книги не должны быть пустой строкой");
                        if (attemptCount == 0)
                        {
                            Console.WriteLine("У вас закончились попытки для входа. Сессия будет принудительно закрыта");
                            Environment.Exit(0);
                        }
                        Console.WriteLine("Вам будет предоставлена еще одна попытка. Осталось попыток: {0}", attemptCount);
                        continue;
                    }
                    else break;
                }               
                foreach (var bookId in inputString.Split(' '))
                {
                    bookIds.Add(Convert.ToInt32(bookId));
                }

                //Считывание выбранной даты доставки
                attemptCount = 4;
                while(attemptCount != 0)
                {
                    Console.WriteLine("Введите дату доставки в одном из указанных форматах: dd/mm/yyyy | dd.mm.yyyy");
                    inputString = Console.ReadLine();
                    if (inputString == "stop")
                    {
                        while (attemptCount != 0)
                        {
                            Console.WriteLine("Вы уверены, что хотите закончить сеанс?[y,n]");
                            inputString = Console.ReadLine();
                            if (inputString == "y") Environment.Exit(0);
                            else if (inputString == "n") break;
                            else
                            {
                                attemptCount--;
                                Console.WriteLine("Вы ввели несуществующую команду");
                                if (attemptCount == 0)
                                {
                                    Console.WriteLine("У вас закончились попытки для входа. Сессия будет принудительно закрыта");
                                    Environment.Exit(0);
                                }
                                Console.WriteLine("Вам будет предоставлена еще одна попытка. Осталось попыток: {0}", attemptCount);
                                continue;
                            }
                        }
                        continue;
                    }
                    else if (String.IsNullOrWhiteSpace(inputString))
                    {
                        attemptCount--;
                        Console.WriteLine("Дата доставки не должна быть пустой строкой");
                        if (attemptCount == 0)
                        {
                            Console.WriteLine("У вас закончились попытки для входа. Сессия будет принудительно закрыта");
                            Environment.Exit(0);
                        }
                        Console.WriteLine("Вам будет предоставлена еще одна попытка. Осталось попыток: {0}", attemptCount);
                        continue;
                    }
                    else if (!ValidationService.IsValidDateTimeFormat(inputString))
                    {
                        Console.WriteLine("Дата доставки должна быть в одном из указанных форматах");
                        attemptCount--;
                        if (attemptCount == 0)
                        {
                            Console.WriteLine("У вас закончились попытки для входа. Сессия будет принудительно закрыта");
                            Environment.Exit(0);
                        }
                        Console.WriteLine("Вам будет предоставлена еще одна попытка. Осталось попыток: {0}", attemptCount);
                        continue;
                    }
                    else if (!ValidationService.IsValidDeleveryDateTime(Convert.ToDateTime(inputString)))
                    {
                        Console.WriteLine("Дата доставки должна быть позже даты заказа хотя бы на 1 день");
                        attemptCount--;
                        if (attemptCount == 0)
                        {
                            Console.WriteLine("У вас закончились попытки для входа. Сессия будет принудительно закрыта");
                            Environment.Exit(0);
                        }
                        Console.WriteLine("Вам будет предоставлена еще одна попытка. Осталось попыток: {0}", attemptCount);
                        continue;
                    }
                    else break;
                }    
                deleveryDate = Convert.ToDateTime(inputString);

                //Вывод доступных способов оплаты
                Console.WriteLine();
                Console.WriteLine("Доступные способы оплаты:");
                foreach (int value in Enum.GetValues(typeof(PaymentTypeEnum)))
                {
                    Console.WriteLine("{0, -5}{1, -15}", value, Enum.GetName(typeof(PaymentTypeEnum), value));
                }
                Console.WriteLine();

                //Считывание id выбранного способа оплаты
                attemptCount = 4;
                while(attemptCount != 0)
                {
                    Console.WriteLine("Выберите один из способ оплаты");
                    inputString = Console.ReadLine();
                    if (inputString == "stop")
                    {
                        while (attemptCount != 0)
                        {
                            Console.WriteLine("Вы уверены, что хотите закончить сеанс?[y,n]");
                            inputString = Console.ReadLine();
                            if (inputString == "y") Environment.Exit(0);
                            else if (inputString == "n") break;
                            else
                            {
                                attemptCount--;
                                Console.WriteLine("Вы ввели несуществующую команду");
                                if (attemptCount == 0)
                                {
                                    Console.WriteLine("У вас закончились попытки для входа. Сессия будет принудительно закрыта");
                                    Environment.Exit(0);
                                }
                                Console.WriteLine("Вам будет предоставлена еще одна попытка. Осталось попыток: {0}", attemptCount);
                                continue;
                            }
                        }
                        continue;
                    }
                    else if (String.IsNullOrWhiteSpace(inputString))
                    {
                        attemptCount--;
                        Console.WriteLine("Способ оплаты не должен быть пустой строкой");
                        if (attemptCount == 0)
                        {
                            Console.WriteLine("У вас закончились попытки для входа. Сессия будет принудительно закрыта");
                            Environment.Exit(0);
                        }
                        Console.WriteLine("Вам будет предоставлена еще одна попытка. Осталось попыток: {0}", attemptCount);
                        continue;
                    }
                    else break;
                }
                paymentTypeId = Convert.ToInt32(inputString);

                paymentStatusId = (int)PaymentStatusEnum.UnPaid;

                //Имитация процесса оплаты заказа, в случае выбора способа оплаты - Картой на сайте
                if (paymentTypeId == (int)PaymentTypeEnum.CardOnline)
                {
                    attemptCount = 4;
                    while(attemptCount != 0)
                    {
                        Console.WriteLine("Вы желаете оплатить заказ сейчас?[y,n]");
                        inputString = Console.ReadLine();
                        if (inputString == "stop")
                        {
                            while (attemptCount != 0)
                            {
                                Console.WriteLine("Вы уверены, что хотите закончить сеанс?[y,n]");
                                inputString = Console.ReadLine();
                                if (inputString == "y") Environment.Exit(0);
                                else if (inputString == "n") break;
                                else
                                {
                                    attemptCount--;
                                    Console.WriteLine("Вы ввели несуществующую команду");
                                    if (attemptCount == 0)
                                    {
                                        Console.WriteLine("У вас закончились попытки для входа. Сессия будет принудительно закрыта");
                                        Environment.Exit(0);
                                    }
                                    Console.WriteLine("Вам будет предоставлена еще одна попытка. Осталось попыток: {0}", attemptCount);
                                    continue;
                                }
                            }
                            continue;
                        }
                        else if (inputString == "y")
                        {
                            Console.WriteLine("Имитация операции оплаты ...");
                            Thread.Sleep(3000);
                            paymentStatusId = (int)PaymentStatusEnum.Paid;
                            break;
                        }
                        else if (inputString == "n")
                        {
                            Console.WriteLine("Вы должны оплатить заказ на сайте до даты доставки");
                            break;
                        }
                    }           
                }

                var newOrder = new Order
                {
                    UserId = user.Id,
                    PaymentTypeId = paymentTypeId,
                    PaymentStatusId = paymentStatusId,
                    DeleveryDate = deleveryDate,
                    OrderDate = DateTime.Now
                };

                //Создание заказа
                try
                {
                    createdOrder = await orderRepository.CreateOrderAsync(newOrder, bookIds);
                    Console.WriteLine();
                    Console.WriteLine("Вам был создан следующий заказ:");
                    Console.WriteLine();

                    Console.WriteLine("{0, -15}{1, -30}{2, -20}{3, -20}{4, -20}{5, -20}{6, -15}{7, -10}", 
                            "Номер заказа", "Заказчик", "Дата заказа", "Дата доставки", "Способ оплаты", "Статус оплаты", "Статус заказ", "Сумма"
                                     );
                   
                    Console.WriteLine("{0, -15}{1, -15}{2, -15}{3, -20}{4, -20}{5, -20}{6, -20}{7, -15}{8, -10: #,###.##}", 
                            createdOrder.Id, createdOrder.User.FirstName, createdOrder.User.LastName, createdOrder.OrderDate.ToShortDateString(), 
                            createdOrder.DeleveryDate.ToLocalTime(), createdOrder.PaymentType.TypeName, createdOrder.PaymentStatus.StatusName, 
                            createdOrder.OrderStatus.StatusName, createdOrder.TotalPrice
                                     );
                }
                catch(SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    throw;
                }

                Console.WriteLine("Желаете повторить операцию[y,n]");
                inputString = Console.ReadLine();
                if (inputString == "stop") return;
                if (inputString == "n") return;
                if (inputString == "y")
                {
                    Console.Clear();
                    continue;
                }
            }
        }

        private async Task<User> AuthenticationProccessAsync()
        {
            var user = new User();
            int attemptCount;
            string inputString;

            Console.WriteLine("Прежде, чем начать сессию, необходимо войти в систему");

            attemptCount = 6;
            while(attemptCount != 0)
            {
                try
                {
                    Console.WriteLine("Вы уже зарегестрированны в системе?[y,n]");
                    inputString = Console.ReadLine();
                    if (inputString == "stop")
                    {
                        while (attemptCount != 0)
                        {
                            Console.WriteLine("Вы уверены, что хотите закончить сеанс?[y,n]");
                            inputString = Console.ReadLine();
                            if (inputString == "y") Environment.Exit(0);
                            else if (inputString == "n") break;
                            else
                            {
                                attemptCount--;
                                Console.WriteLine("Вы ввели несуществующую команду");
                                if (attemptCount == 0)
                                {
                                    Console.WriteLine("У вас закончились попытки для входа. Сессия будет принудительно закрыта");
                                    Environment.Exit(0);
                                }
                                Console.WriteLine("Вам будет предоставлена еще одна попытка. Осталось попыток: {0}", attemptCount);
                                continue;
                            }
                        }
                        continue;
                    }
                    
                    //Процесс аутентификации
                    if (inputString == "y")
                    {
                        while (attemptCount != 0)
                        {
                            Console.WriteLine("Введите свою почту для аутентификации");
                            inputString = Console.ReadLine();
                            if (inputString == "stop")
                            {
                                while (attemptCount != 0)
                                {
                                    Console.WriteLine("Вы уверены, что хотите закончить сеанс?[y,n]");
                                    inputString = Console.ReadLine();
                                    if (inputString == "y") Environment.Exit(0);
                                    else if (inputString == "n") break;
                                    else
                                    {
                                        attemptCount--;
                                        Console.WriteLine("Вы ввели несуществующую команду");
                                        if (attemptCount == 0)
                                        {
                                            Console.WriteLine("У вас закончились попытки для входа. Сессия будет принудительно закрыта");
                                            Environment.Exit(0);
                                        }
                                        Console.WriteLine("Вам будет предоставлена еще одна попытка. Осталось попыток: {0}", attemptCount);
                                        continue;
                                    }
                                }
                                continue;
                            }
                            if (ValidationService.IsValidEmail(inputString))
                            {
                                if (await userRepository.IsRegisteredAsync(inputString))
                                {
                                    user = await userRepository.GetUserByEmailAsync(inputString);
                                    if (user == null) throw new ArgumentNullException("User in null");
                                    return user;
                                }
                                else
                                {
                                    attemptCount--;
                                    Console.WriteLine("Пользователь с таким email не зарегистрирован в системе");
                                    if (attemptCount == 0)
                                    {
                                        Console.WriteLine("У вас закончились попытки для входа. Сессия будет принудительно закрыта");
                                        Environment.Exit(0);
                                    }

                                    Console.WriteLine("Хотите повторно попытаться войти в систему?[y,n]");
                                    inputString = Console.ReadLine();
                                    if (inputString == "stop")
                                    {
                                        while (attemptCount != 0)
                                        {
                                            Console.WriteLine("Вы уверены, что хотите закончить сеанс?[y,n]");
                                            inputString = Console.ReadLine();
                                            if (inputString == "y") Environment.Exit(0);
                                            else if (inputString == "n") break;
                                            else
                                            {
                                                attemptCount--;
                                                Console.WriteLine("Вы ввели несуществующую команду");
                                                if (attemptCount == 0)
                                                {
                                                    Console.WriteLine("У вас закончились попытки для входа. Сессия будет принудительно закрыта");
                                                    Environment.Exit(0);
                                                }
                                                Console.WriteLine("Вам будет предоставлена еще одна попытка. Осталось попыток: {0}", attemptCount);
                                                continue;
                                            }
                                        }
                                        continue;
                                    }
                                    else if (inputString == "y") continue;
                                    else if (inputString == "n") break;
                                    else
                                    {
                                        attemptCount--;
                                        Console.WriteLine("Вы ввели несуществующую команду");
                                        if (attemptCount == 0)
                                        {
                                            Console.WriteLine("У вас закончились попытки для входа. Сессия будет принудительно закрыта");
                                            Environment.Exit(0);
                                        }
                                        Console.WriteLine("Вам будет предоставлена еще одна попытка. Осталось попыток: {0}", attemptCount);
                                        continue;
                                    }
                                }
                            }
                            else
                            {
                                attemptCount--;
                                Console.WriteLine();
                                Console.WriteLine("Вы ввели почту в недопустимом формате");
                                if (attemptCount == 0)
                                {
                                    Console.WriteLine("У вас закончились попытки для входа. Сессия будет принудительно закрыта");
                                    Environment.Exit(0);
                                }
                                Console.WriteLine("Вам будет предоставлена еще одна попытка. Осталось попыток: {0}", attemptCount);
                                Console.WriteLine();
                                continue;
                            }
                        }
                        inputString = "n";
                    }
                    
                    //Процесс регистрации
                    if (inputString == "n")
                    {
                        Console.WriteLine("В системе могут работать только авторизированные пользователи");
                        Console.WriteLine("Поэтому, прежде вам необходимо пройти процесс регистрации");
                        user = await RegistrationProccessAsync();                      
                    }
                    else
                    {
                        attemptCount--;
                        Console.WriteLine("Вы ввели несуществующую команду");
                        if (attemptCount == 0)
                        {
                            Console.WriteLine("У вас закончились попытки для входа. Сессия будет принудительно закрыта");
                            Environment.Exit(0);
                        }
                        Console.WriteLine("Вам будет предоставлена еще одна попытка. Осталось попыток: {0}", attemptCount);
                        continue;
                    }
                }
                catch(Exception ex)
                {
                    throw;
                }              
            }
            return user;
        }

        private async Task<User> RegistrationProccessAsync()
        {

            string inputString = String.Empty;
            int attemptCount;
            var user = new User();
            
            //Считывание введенного имени
            attemptCount = 4;
            while(attemptCount != 0)
            {
                Console.WriteLine("Введите свое имя");
                inputString = Console.ReadLine();
                if (String.IsNullOrWhiteSpace(inputString))
                {
                    attemptCount--;
                    Console.WriteLine("Имя пользователя не должны быть пустой строкой");
                    if (attemptCount == 0)
                    {
                        Console.WriteLine("У вас закончились попытки для входа. Сессия будет принудительно закрыта");
                        Environment.Exit(0);
                    }
                    Console.WriteLine("Вам будет предоставлена еще одна попытка. Осталось попыток: {0}", attemptCount);
                    continue;
                }
                else if (inputString == "stop")
                {
                    while (attemptCount != 0)
                    {
                        Console.WriteLine("Вы уверены, что хотите закончить сеанс?[y,n]");
                        inputString = Console.ReadLine();
                        if (inputString == "y") Environment.Exit(0);
                        else if (inputString == "n") break;
                        else
                        {
                            attemptCount--;
                            Console.WriteLine("Вы ввели несуществующую команду");
                            if (attemptCount == 0)
                            {
                                Console.WriteLine("У вас закончились попытки для входа. Сессия будет принудительно закрыта");
                                Environment.Exit(0);
                            }
                            Console.WriteLine("Вам будет предоставлена еще одна попытка. Осталось попыток: {0}", attemptCount);
                            continue;
                        }
                    }
                    continue;
                }
                else
                {
                    user.FirstName = inputString;
                    break;
                }                
            }

            //Считывание введенной фамилии 
            attemptCount = 4;
            while (attemptCount != 0)
            {
                Console.WriteLine("Введите Фамилию");
                inputString = Console.ReadLine();
                if (String.IsNullOrWhiteSpace(inputString))
                {
                    attemptCount--;
                    Console.WriteLine("Фамилия пользователя не должна быть пустой строкой");
                    if (attemptCount == 0)
                    {
                        Console.WriteLine("У вас закончились попытки для входа. Сессия будет принудительно закрыта");
                        Environment.Exit(0);
                    }
                    Console.WriteLine("Вам будет предоставлена еще одна попытка. Осталось попыток: {0}", attemptCount);
                    continue;
                }
                else if (inputString == "stop")
                {
                    while (attemptCount != 0)
                    {
                        Console.WriteLine("Вы уверены, что хотите закончить сеанс?[y,n]");
                        inputString = Console.ReadLine();
                        if (inputString == "y") Environment.Exit(0);
                        else if (inputString == "n") break;
                        else
                        {
                            attemptCount--;
                            Console.WriteLine("Вы ввели несуществующую команду");
                            if (attemptCount == 0)
                            {
                                Console.WriteLine("У вас закончились попытки для входа. Сессия будет принудительно закрыта");
                                Environment.Exit(0);
                            }
                            Console.WriteLine("Вам будет предоставлена еще одна попытка. Осталось попыток: {0}", attemptCount);
                            continue;
                        }
                    }
                    continue;
                }
                else
                {
                    user.LastName = inputString;
                    break;
                }              
            }

            //Считывание введенного номера телефона 
            attemptCount = 4;
            while (attemptCount != 0)
            {
                Console.WriteLine("Введите номер телефона в формате: +7xxx xxx xx xx  или 8xxx xxx xx xx ");
                inputString = Console.ReadLine().Replace(" ", "");
                if (String.IsNullOrWhiteSpace(inputString))
                {
                    attemptCount--;
                    Console.WriteLine("Номер телефона пользователя должен удовлетворять указанному шаблону");
                    if (attemptCount == 0)
                    {
                        Console.WriteLine("У вас закончились попытки для входа. Сессия будет принудительно закрыта");
                        Environment.Exit(0);
                    }
                    Console.WriteLine("Вам будет предоставлена еще одна попытка. Осталось попыток: {0}", attemptCount);
                    continue;
                }
                else if (!ValidationService.IsValidPhoneNumber(inputString))
                {
                    attemptCount--;
                    Console.WriteLine("Номер телефона пользователя должен быть валидным");
                    if (attemptCount == 0)
                    {
                        Console.WriteLine("У вас закончились попытки для входа. Сессия будет принудительно закрыта");
                        Environment.Exit(0);
                    }
                    Console.WriteLine("Вам будет предоставлена еще одна попытка. Осталось попыток: {0}", attemptCount);
                    continue;
                }
                else if (inputString == "stop")
                {
                    while (attemptCount != 0)
                    {
                        Console.WriteLine("Вы уверены, что хотите закончить сеанс?[y,n]");
                        inputString = Console.ReadLine();
                        if (inputString == "y") Environment.Exit(0);
                        else if (inputString == "n") break;
                        else
                        {
                            attemptCount--;
                            Console.WriteLine("Вы ввели несуществующую команду");
                            if (attemptCount == 0)
                            {
                                Console.WriteLine("У вас закончились попытки для входа. Сессия будет принудительно закрыта");
                                Environment.Exit(0);
                            }
                            Console.WriteLine("Вам будет предоставлена еще одна попытка. Осталось попыток: {0}", attemptCount);
                            continue;
                        }
                    }
                    continue;
                }
                else
                {
                    user.PhoneNumber = inputString;
                    break;
                }
            }

            //Считывание введенного email 
            attemptCount = 4;
            while (attemptCount != 0)
            {
                Console.WriteLine("Введите email");
                inputString = Console.ReadLine();
                if (!ValidationService.IsValidEmail(inputString))
                {
                    attemptCount--;
                    Console.WriteLine("Email пользователя должен быть валидным");
                    if (attemptCount == 0)
                    {
                        Console.WriteLine("У вас закончились попытки для входа. Сессия будет принудительно закрыта");
                        Environment.Exit(0);
                    }
                    Console.WriteLine("Вам будет предоставлена еще одна попытка. Осталось попыток: {0}", attemptCount);
                    continue;
                }
                else if (inputString == "stop")
                {
                    while (attemptCount != 0)
                    {
                        Console.WriteLine("Вы уверены, что хотите закончить сеанс?[y,n]");
                        inputString = Console.ReadLine();
                        if (inputString == "y") Environment.Exit(0);
                        else if (inputString == "n") break;
                        else
                        {
                            attemptCount--;
                            Console.WriteLine("Вы ввели несуществующую команду");
                            if (attemptCount == 0)
                            {
                                Console.WriteLine("У вас закончились попытки для входа. Сессия будет принудительно закрыта");
                                Environment.Exit(0);
                            }
                            Console.WriteLine("Вам будет предоставлена еще одна попытка. Осталось попыток: {0}", attemptCount);
                            continue;
                        }
                    }
                    continue;
                }
                else
                {
                    user.Email = inputString;
                    break;
                }
            }

            //Считывание введенной страны проживания 
            attemptCount = 4;
            while (attemptCount != 0)
            {
                Console.WriteLine("Введите страну проживания");
                inputString = Console.ReadLine();
                if (String.IsNullOrWhiteSpace(inputString))
                {
                    attemptCount--;
                    Console.WriteLine("Страна проживания пользователя не должна быть пустой строкой");
                    if (attemptCount == 0)
                    {
                        Console.WriteLine("У вас закончились попытки для входа. Сессия будет принудительно закрыта");
                        Environment.Exit(0);
                    }
                    Console.WriteLine("Вам будет предоставлена еще одна попытка. Осталось попыток: {0}", attemptCount);
                    continue;
                }
                else if (inputString == "stop")
                {
                    while (attemptCount != 0)
                    {
                        Console.WriteLine("Вы уверены, что хотите закончить сеанс?[y,n]");
                        inputString = Console.ReadLine();
                        if (inputString == "y") Environment.Exit(0);
                        else if (inputString == "n") break;
                        else
                        {
                            attemptCount--;
                            Console.WriteLine("Вы ввели несуществующую команду");
                            if (attemptCount == 0)
                            {
                                Console.WriteLine("У вас закончились попытки для входа. Сессия будет принудительно закрыта");
                                Environment.Exit(0);
                            }
                            Console.WriteLine("Вам будет предоставлена еще одна попытка. Осталось попыток: {0}", attemptCount);
                            continue;
                        }
                    }
                    continue;
                }
                else
                {
                    user.Country = inputString;
                    break;
                }
            }

            //Считывание введенного города проживания
            attemptCount = 4;
            while (attemptCount != 0)
            {
                Console.WriteLine("Введите город проживания");
                inputString = Console.ReadLine();
                if (String.IsNullOrWhiteSpace(inputString))
                {
                    attemptCount--;
                    Console.WriteLine("Город проживания пользователя не должен быть пустой строкой");
                    if (attemptCount == 0)
                    {
                        Console.WriteLine("У вас закончились попытки для входа. Сессия будет принудительно закрыта");
                        Environment.Exit(0);
                    }
                    Console.WriteLine("Вам будет предоставлена еще одна попытка. Осталось попыток: {0}", attemptCount);
                    continue;
                }
                else if (inputString == "stop")
                {
                    while (attemptCount != 0)
                    {
                        Console.WriteLine("Вы уверены, что хотите закончить сеанс?[y,n]");
                        inputString = Console.ReadLine();
                        if (inputString == "y") Environment.Exit(0);
                        else if (inputString == "n") break;
                        else
                        {
                            attemptCount--;
                            Console.WriteLine("Вы ввели несуществующую команду");
                            if (attemptCount == 0)
                            {
                                Console.WriteLine("У вас закончились попытки для входа. Сессия будет принудительно закрыта");
                                Environment.Exit(0);
                            }
                            Console.WriteLine("Вам будет предоставлена еще одна попытка. Осталось попыток: {0}", attemptCount);
                            continue;
                        }
                    }
                    continue;
                }
                else
                {
                    user.City = inputString;
                    break;
                }
            }

            //Считывание введенной улицы проживания
            attemptCount = 4;
            while (attemptCount != 0)
            {
                Console.WriteLine("Введите улицу проживания");
                inputString = Console.ReadLine();
                if (String.IsNullOrWhiteSpace(inputString))
                {
                    attemptCount--;
                    Console.WriteLine("Улица проживания пользователя не должна быть пустой строкой");
                    if (attemptCount == 0)
                    {
                        Console.WriteLine("У вас закончились попытки для входа. Сессия будет принудительно закрыта");
                        Environment.Exit(0);
                    }
                    Console.WriteLine("Вам будет предоставлена еще одна попытка. Осталось попыток: {0}", attemptCount);
                    continue;
                }
                else if (inputString == "stop")
                {
                    while (attemptCount != 0)
                    {
                        Console.WriteLine("Вы уверены, что хотите закончить сеанс?[y,n]");
                        inputString = Console.ReadLine();
                        if (inputString == "y") Environment.Exit(0);
                        else if (inputString == "n") break;
                        else
                        {
                            attemptCount--;
                            Console.WriteLine("Вы ввели несуществующую команду");
                            if (attemptCount == 0)
                            {
                                Console.WriteLine("У вас закончились попытки для входа. Сессия будет принудительно закрыта");
                                Environment.Exit(0);
                            }
                            Console.WriteLine("Вам будет предоставлена еще одна попытка. Осталось попыток: {0}", attemptCount);
                            continue;
                        }
                    }
                    continue;
                }
                else
                {
                    user.Street = inputString;
                    break;
                }
            }

            //Считывание введенного номера дома
            attemptCount = 4;
            while (attemptCount != 0)
            {
                Console.WriteLine("Введите номер дома");
                inputString = Console.ReadLine();
                if (String.IsNullOrWhiteSpace(inputString))
                {
                    attemptCount--;
                    Console.WriteLine("Номер дома пользователя не должен быть пустой строкой");
                    if (attemptCount == 0)
                    {
                        Console.WriteLine("У вас закончились попытки для входа. Сессия будет принудительно закрыта");
                        Environment.Exit(0);
                    }
                    Console.WriteLine("Вам будет предоставлена еще одна попытка. Осталось попыток: {0}", attemptCount);
                    continue;
                }
                else if(inputString == "stop")
                {
                    while (attemptCount != 0)
                    {
                        Console.WriteLine("Вы уверены, что хотите закончить сеанс?[y,n]");
                        inputString = Console.ReadLine();
                        if (inputString == "y") Environment.Exit(0);
                        else if (inputString == "n") break;
                        else
                        {
                            attemptCount--;
                            Console.WriteLine("Вы ввели несуществующую команду");
                            if (attemptCount == 0)
                            {
                                Console.WriteLine("У вас закончились попытки для входа. Сессия будет принудительно закрыта");
                                Environment.Exit(0);
                            }
                            Console.WriteLine("Вам будет предоставлена еще одна попытка. Осталось попыток: {0}", attemptCount);
                            continue;
                        }
                    }
                    continue;
                }
                else
                {
                    user.BuildingNumber = inputString;
                    break;
                }
            }

            //Считывание введенного номера квартиры, если пользователь проживает в многоквартирном доме
            attemptCount = 4;
            while(attemptCount != 0)
            {
                Console.WriteLine("Вы живете в многоквартирном доме?[y,n]");
                inputString = Console.ReadLine();
                if(inputString == "stop")
                {
                    while(attemptCount != 0)
                    {
                        Console.WriteLine("Вы уверены, что хотите закончить сеанс?[y,n]");
                        inputString = Console.ReadLine();
                        if (inputString == "y") Environment.Exit(0);
                        else if (inputString == "n") break;
                        else
                        {
                            attemptCount--;
                            Console.WriteLine("Вы ввели несуществующую команду");
                            if (attemptCount == 0)
                            {
                                Console.WriteLine("У вас закончились попытки для входа. Сессия будет принудительно закрыта");
                                Environment.Exit(0);
                            }
                            Console.WriteLine("Вам будет предоставлена еще одна попытка. Осталось попыток: {0}", attemptCount);
                            continue;
                        }
                    }
                    continue;
                }
                else if (inputString == "y")
                {                 
                    while(attemptCount != 0)
                    {
                        Console.WriteLine("Введите номер квартиры");
                        inputString = Console.ReadLine();
                        if (String.IsNullOrWhiteSpace(inputString))
                        {
                            attemptCount--;
                            Console.WriteLine("Номер квартиры пользователя не должен быть пустой строкой");
                            if (attemptCount == 0)
                            {
                                Console.WriteLine("У вас закончились попытки для входа. Сессия будет принудительно закрыта");
                                Environment.Exit(0);
                            }
                            Console.WriteLine("Вам будет предоставлена еще одна попытка. Осталось попыток: {0}", attemptCount);
                            continue;
                        }
                        else if (inputString == "stop")
                        {
                            while (attemptCount != 0)
                            {
                                Console.WriteLine("Вы уверены, что хотите закончить сеанс?[y,n]");
                                inputString = Console.ReadLine();
                                if (inputString == "y") Environment.Exit(0);
                                else if (inputString == "n") break;
                                else
                                {
                                    attemptCount--;
                                    Console.WriteLine("Вы ввели несуществующую команду");
                                    if (attemptCount == 0)
                                    {
                                        Console.WriteLine("У вас закончились попытки для входа. Сессия будет принудительно закрыта");
                                        Environment.Exit(0);
                                    }
                                    Console.WriteLine("Вам будет предоставлена еще одна попытка. Осталось попыток: {0}", attemptCount);
                                    continue;
                                }
                            }
                            continue;
                        }
                        else
                        {
                            user.FlatNumber = inputString;
                            break;
                        }
                    }
                   
                }
                else if (inputString == "n")
                {
                    break;
                }
                else
                {
                    attemptCount--;
                    Console.WriteLine("Вы ввели несуществующую команду");
                    if (attemptCount == 0)
                    {
                        Console.WriteLine("У вас закончились попытки для входа. Сессия будет принудительно закрыта");
                        Environment.Exit(0);
                    }
                    Console.WriteLine("Вам будет предоставлена еще одна попытка. Осталось попыток: {0}", attemptCount);
                    continue;
                }            
            }

            //Создание нового пользователя
            try
            {              
                var createdUser =  await userRepository.CreateUserAsync(user);
                if (createdUser == null) throw new ArgumentNullException("Created user is null");
                return createdUser;
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}

