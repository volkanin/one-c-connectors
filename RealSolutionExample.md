решение конкретных задач с помощью нашего проекта

## простейший обмен сообщениями (инкрементальный гарантированный обмен) ##

**описание задачи**

  * существует один главный узел и один подчиненный узел распределённой ИБ 1С 8.1
  * необходимо по "патерну" [request-reply](http://www.enterpriseintegrationpatterns.com/RequestReply.html) реализовать следующую задачу
    * главный узел
      * открывает выборку изменений для подчиненного узла
      * получает одно изменение
      * записывает изменение в сообщение
      * отправляет
    * распределенный узел
      * получает сообщение
      * записывает себе одно изменение данных из главного узла
      * открывает выборку своих изменений
      * получает одно изменение
      * отправляет в качестве ответа главному сообщение
    * главный узел записывает полученное сообщение себе
  * для простоты задачи будем использовать два "коннекта" для службы - главный узел на сервере, распределённый - файловый (каталог доступен относительно сервера)
  * вызывающим контекстом будет главный узел
  * у узла плана обмена есть два дополнительных реквизита: URL службы, Имя Соединения для узла из коллекции соединений Службы
  * запуск будет осуществляться каждую минуту через регламентное задание

**реализация**

  * //TODO пример реализации надо добавить в репозитарий
  * ...