#pragma once
#include "user.h"
/*
	Add_question
	Send_message
	Read_message

*/
class site
{
	vector<user> User;
public:
	site();
	void start();
	void read();
	void store();
	void Login();
	void Register();
	void SendMessageToAllUsers(int currentIndex);
	void UserOptions(int currentIndex);
	void Qeustion_options(int currentIndex);
	void Send_options(int currentIndex);
	void Read_options(int currentIndex);
	void ViewAllContacts(int currentIndex);
	bool SearchAboutContact(int currentIndex, int sender_id);
	void RemoveContact(int currentIndex);
	void Send_Qestion(int currentIndex);
	void Add_user_contacts(int currentIndex);
	void PutFavoritMessage(int currentIndex);
	void RemoveFavoritMessage(int currentIndex);
	void send_message(int currentIndex);
	void Undo_message(int currentIndex);
	void Undo_qeustion(int currentIndex);
	void ViewAllMyQeustion(int currentIndex);
	void ViewAllSentMessage(int currentIndex);
	void ViewAllrecipientMessage(int currentIndex);
	void ViewAllrecipientQeustion(int currentIndex);
	void ViewAllfavoritMessage(int currentIndex);
	~site();
};

