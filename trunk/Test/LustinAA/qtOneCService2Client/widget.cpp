#include "widget.h"
#include "ui_widget.h"

Widget::Widget(QWidget *parent) :
    QWidget(parent),
    ui(new Ui::Widget),
    http(this)
{
    ui->setupUi(this);

    http.setHost("http://127.0.0.1:9000/OneCService2",9000);
}

Widget::~Widget()
{
    delete ui;
}

void Widget::changeEvent(QEvent *e)
{
    QWidget::changeEvent(e);
    switch (e->type()) {
    case QEvent::LanguageChange:
        ui->retranslateUi(this);
        break;
    default:
        break;
    }
}

void Widget::on_pushButton_clicked()
{
    // Construct a method request message.
    QtSoapMessage request;

        // Set the method and add one argument.
    request.setMethod("ExecuteScript", "http://OneCService2");
    request.addMethodArgument("_connectionName", "", "Test81");
    request.addMethodArgument("_poolUserName", "", "PoolUserName");
    request.addMethodArgument("_poolPassword", "", "PoolPassword");
    request.addMethodArgument("_script", "", "ТекущаяДата();");

        // Submit the request the the web service.
    http.setHost("127.0.0.1",9000);
    http.setAction("127.0.0.1:9000/OneCService2");
    http.submitRequest(request, "");

    const QtSoapMessage &message = http.getResponse();

    ui->textBrowser->setHtml(message.toXmlString());

}
