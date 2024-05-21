#include "user.h"
Sentmessages::Sentmessages()
{
	id_messsage = 0;
	id_user = 0;
	message = " ";
}
user::user() {
	id = 1;
}
user::~user()
{
	contact.clear();
	sentmessage.clear();
	recipientmessage.clear();
	sentquestions.clear();
	recipientquestions.clear();
}
user::user(int id, string username, string password)
{
	this->id = id;
	this->username = username;
	this->password = password;
}

