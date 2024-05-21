#include "site.h"
#include <string>    // ctrl r
#include <conio.h> //getline
#include <fstream>            /// read and write 
#include <iostream>
#include <assert.h>
#include<Windows.h>  //for edit console
using namespace std;
HANDLE h = GetStdHandle(STD_OUTPUT_HANDLE); //initialization h variable
			//ok
site::site()
{
	read();
	start();
	store();
}
			//ok
void site::start()
{
	SetConsoleTextAttribute(h, 9);
	cout << "\n\t\t\t\t\t\t  * * * * * * * * * * * * * * * * * *  *\n";
	cout << "\t\t\t\t\t\t  *   Welcome in our modified saraha   *\n";
	cout << "\t\t\t\t\t\t  * * * * * * * * * * * * * * * * * *  *\n";
	cout << "\t\t\t\t\t\t  * you can send a message to any User *\n\t\t\t\t\t\t  *    no any one know who are you     *\n";
	cout << "\t\t\t\t\t\t  *                                    *\n";
	cout << "\t\t\t\t\t\t  * * * * * * * * * * * * * * * * * *  *\n";
	int choice = 0;
	SetConsoleTextAttribute(h, 11);// 7,8white
	cout << "\n\t\t\t\t\t\t\t[1].LOGIN\n";
	cout << "\n\t\t\t\t\t\t\t[2].Register\n";
	cout << "\n\t\t\t\t\t\t\t[3].Exit\n\n";
	cout << "\t\t\t\t\t\t\tEnter your chioce:  ";
	cin >> choice;
	switch (choice)
	{
	case 1:
		Login();
		break;
	case 2:
		Register();
		break;
	case 3:
	{
		system("cls");
		cout << "\n\t\t\t\t\t__________Thank you for using our Application______________\n\n"
			<< "\t\t\t\t\t\t  press any key to close completely\n\n\t\t\t\t\t\t  ";
		break;
	}
	default:
	{
		system("cls");
		cout << "\n\t\t\t\t\t__________wrong chioce, please try again ______________\n\n";
		start();
	}

	}
}
			//ok//nada
void site::read()
{
	/*
		read all data from the files
	*/
	user u1;
	string id_user, id_mes, mes, seperator, seperator1,fav;
	int iduser, idmes;
	Sentmessages sent_mes;
	ifstream read("user.txt");
	ifstream recipientques("recipientques.txt");
	ifstream recipientmes("recipientmes.txt");
	ifstream favoritemes("favoritemes.txt");
	ifstream sentquestion("sentques.txt");
	for (int i = 0; !read.eof(); i++) {
		read >> u1.id >> u1.username >> u1.password;
		User.push_back(u1);
		while (true) {
			read >> idmes >> iduser >> seperator;
			User[i].contact.emplace(idmes, iduser);
			if (seperator == "+") { // - anther message ,//+ anther user
				break;
			}
		}
	}
	
	for (int i = 0; i < User.size(); i++) {
		for (int j = 0; !recipientques.eof(); j++) {
			getline(recipientques, id_user);
			getline(recipientques, id_mes);
			getline(recipientques, sent_mes.message);
			getline(recipientques, seperator);
			if (recipientques.eof()) { break; }
			sent_mes.id_user = stoi(id_user);
			sent_mes.id_messsage = stoi(id_mes);
			User[i].recipientquestions.push_back(sent_mes);
			//User[sent_mes.id_user-1].sentquestions.push_back(sent_mes.message);
			if (seperator == "+") { // - anther message ,//+ anther user
				break;
			}
		}
	}
	for (int i = 0; i < User.size(); i++) {
		for (int j = 0; !recipientmes.eof(); j++) {
			getline(recipientmes, id_user);
			getline(recipientmes, id_mes);
			getline(recipientmes, sent_mes.message);
			getline(recipientmes, seperator);
			if (recipientmes.eof()) { break; }
			sent_mes.id_user = stoi(id_user);
			sent_mes.id_messsage = stoi(id_mes);
			User[i].recipientmessage.push_back(sent_mes);
			User[sent_mes.id_user - 1].sentmessage.push_back(sent_mes.message);
			if (seperator == "+") { // - anther message ,//+ anther user
				break;
			}
		}
	}
	for (int i = 0; i < User.size(); i++) {
		for (int j = 0; !favoritemes.eof(); j++) {
			getline(favoritemes, id_mes);
			getline(favoritemes, mes);
			getline(favoritemes, seperator);
			idmes = stoi(id_mes);
			User[i].favoritmessage.emplace(idmes, mes);
			if (seperator == "+") { // - anther message ,//+ anther user
				break;
			}
		}
	}
	for (int i = 0; i < User.size(); i++) {
		for (int j = 0; !sentquestion.eof(); j++) {
			getline(sentquestion, mes);
			getline(sentquestion, seperator);
			User[i].sentquestions.push_back(mes);
			if (seperator == "+") { // - anther message ,//+ anther user
				break;
			}
		}
	}

	read.close();
	recipientques.close();
	recipientmes.close();
	favoritemes.close();
	sentquestion.close();
}
			//ok//nada
void site::store()
{
	/*
		store all data in the files
	*/

	fstream file("user.txt", ios::out);
	fstream recipientques("recipientques.txt", ios::out);
	fstream recipientmes("recipientmes.txt", ios::out);
	fstream favoritemes("favoritemes.txt", ios::out);
	fstream sentques("sentques.txt", ios::out);
	for (int i = 0; i < User.size(); i++)
	{
		file << User[i].id << "\t" << User[i].username << "\t" << User[i].password << "\t";
		auto it = User[i].contact.begin();
		for (int j = 0; j < User[i].contact.size(); j++)
		{
			file << it->first << "\t" << it->second << "\t";
			if (j + 1 == User[i].contact.size()) {
				file << "+";
			}
			else { file << "-" << "\t"; }
			it++;
		}
		if (i == User.size() - 1)
			break;
		file << "\n";
	}
	for (int i = 0; i < User.size(); i++)
	{
		for (int j = 0; j < User[i].recipientquestions.size(); j++)
		{
			recipientques << User[i].recipientquestions[j].id_user << "\n"
				<< User[i].recipientquestions[j].id_messsage << "\n"
				<< User[i].recipientquestions[j].message << "\n";
			if (j + 1 == User[i].recipientquestions.size()) { recipientques << "+" << endl; }
			else { recipientques << "-" << endl; }
		}

		for (int j = 0; j < User[i].recipientmessage.size(); j++)
		{
			recipientmes << User[i].recipientmessage[j].id_user << "\n"
				<< User[i].recipientmessage[j].id_messsage << "\n"
				<< User[i].recipientmessage[j].message << "\n";
			if (j + 1 == User[i].recipientmessage.size()) { recipientmes << "+" << endl; }
			else { recipientmes << "-" << endl; }
		}

		if (!User[i].favoritmessage.empty()) {
			auto f = User[i].favoritmessage.begin();
			for (int j = 0; j < User[i].favoritmessage.size(); j++)
			{
				favoritemes << f->first << "\n" << f->second << "\n";
				if (j + 1 == User[i].favoritmessage.size()) {
					favoritemes << "+";
					if (i == User.size() - 1)
						break;
				}
				else { favoritemes << "-"; }
				favoritemes << "\n";
				f++;

			}
		}
		int counter = 0;
		for (string l : User[i].sentquestions)
		{
			sentques << l << "\n";
			if (counter + 1 == User[i].sentquestions.size()) { sentques << "+" << endl; }
			else { sentques << "-" << endl; }
			counter++;
		}
	}
	file.close();
	recipientques.close();
	recipientmes.close();
	favoritemes.close();
	sentques.close();
}
			//ok//gehad
void site::Login()
{
	bool foundUser = false;
	int currentIndex = 0;
	char ch = {};
	string username, password;
	SetConsoleTextAttribute(h, 12);
	system("cls");
	cout << "\n\t\t\t\t\t\t* * * * \n";
	cout << "\t\t\t\t\t\t*LOGIN*\n";
	cout << "\t\t\t\t\t\t* * * *\n";
	SetConsoleTextAttribute(h, 7);
	cout << "\t\t\t\t\t_______________________";
	cout << "\n\n\t\t\t\t\tEnter username: ";
	cin.ignore();
	getline(cin, username);
	cout << "\t\t\t\t\t_______________________\n\n";
	cout << "\t\t\t\t\tEnter password: ";
	ch = _getch();
	while (ch != '\r')
	{
		password += ch;
		cout << "*";
		ch = _getch();
	}
	for (int i = 0; i < User.size(); i++) {
		if ((User[i].username == username) && (User[i].password == password)) {
			foundUser = true;
			currentIndex = i+1;
			break;
		}
	}
	if (foundUser) {
		SetConsoleTextAttribute(h, 11);
		cout << "\n\n\t\t\t\t\tSuccessful LOGIN\n\t\t\t\t\t";
		system("pause");
		SetConsoleTextAttribute(h, 8);
		UserOptions(currentIndex);
	}
	else
	{
		string answer;
		cout << "\n\t\t\t\t\t_______________________________________\n";
		cout << "\n\t\t\t\t\tusername or password is wrong!!!!\n\n\t\t\t\t\tdo you want to try again? (yes/no) ";
		cin >> answer;
		if (answer == "yes" || answer == "YES" || answer == "Yes")
		{
			Login();
		}
		else {
			system("cls");
			start();
		}
	}
}
			//ok//gehad
void site::Register()
{
	int id;
	if (User.size() == 0) {
		id = 1;
	}
	else {
		id = User[User.size() - 1].id + 1;
	}
	string username, password, rewritePassword = {};
	char ch;	SetConsoleTextAttribute(h, 6);
	system("cls");
	cout << "\n\t\t\t\t\t\t* * * * * * * * \n";
	cout << "\t\t\t\t\t\t* Registration*\n";
	cout << "\t\t\t\t\t\t*             *\n";
	cout << "\t\t\t\t\t\t* * * * * * * * \n\n"; SetConsoleTextAttribute(h, 7);
	cout << "\n\t\t\t\t________________________________________________________________________\n";
	cout << "\n\t\t\t\t\t\tUsername: ";
	cin.ignore();
	getline(cin, username);
	while (true)
	{
		SetConsoleTextAttribute(h, 12);
		cout << "\n\t\t\t\t\tMaximum letters and numbers are 10 and minimum are 5\n"; SetConsoleTextAttribute(h, 7);
		cout << "\t\t\t\t\t\tPassword: ";
		ch = _getch();
		while (ch != '\r')
		{
			password += ch;
			cout << "*";
			ch = _getch();
		}
		if (password.size() > 10 || password.size() < 5)
		{
			password = {};
			continue;
		}
		else
			break;
	}
	while (true)
	{
		cout << "\n\t\t\t\t\t\trewrite password: ";
		ch = _getch();
		while (ch != '\r')
		{
			rewritePassword += ch;
			cout << "*";
			ch = _getch();
		}
		if (rewritePassword != password)
		{
			string answer;
			cout << "\n\n\t\t\t\t\t\tNot matching !!!\n\n\t\t\t\t\t\tdo you want to try again? (yes/no) ";
			cin >> answer;
			if (answer == "yes" || answer == "YES" || answer == "Yes")
			{
				rewritePassword = {};
				continue;
			}
			else
			{
				system("cls");
				start();
				break;
			}
		}
		else {
			cout << "\n\t\t\t\t\t\t___________________________________________________________\n";
			cout << "\n\t\t\t\t\t\tSuccessful Registration\n\t\t\t\t\t\t";
			system("pause");
			user u(id, username, password);
			User.push_back(u);
			SendMessageToAllUsers(id);
			UserOptions(id);
			break;
		}
	}
}
			//ok//nada
void site::SendMessageToAllUsers(int currentIndex)
{
	string ques;
	ques = "Hello, do you want to send me a message?";
	User[currentIndex - 1].sentquestions.push_back(ques);
	Sentmessages m1;
	m1.message = ques;
	m1.id_messsage = 0;
	m1.id_user = currentIndex;
	User[currentIndex - 1].sentquestions.push_back(ques);
	for (int i = 0; i < User.size(); i++) {
		if (i == (currentIndex - 1))
			continue;
		User[i].recipientquestions.push_back(m1);
	}
}
			//ok//gehad
void site::UserOptions(int currentIndex)
{
	system("cls");
	int choice = 0;
	SetConsoleTextAttribute(h, 3);// 7,8white
	cout << "\n\t\t\t\t\t\t\t[1].Ask Qeution\n";
	cout << "\n\t\t\t\t\t\t\t[2].Send message\n";
	cout << "\n\t\t\t\t\t\t\t[3].Read message\n";
	cout << "\n\t\t\t\t\t\t\t[4].View all contacts\n";
	cout << "\n\t\t\t\t\t\t\t[5].Add user in contact\n";
	cout << "\n\t\t\t\t\t\t\t[6].Remove contact\n";
	cout << "\n\t\t\t\t\t\t\t[7].Log out\n\n";
	cout << "\t\t\t\t\t\t\tEnter your choice:  ";
	cin >> choice;
	SetConsoleTextAttribute(h, 7);// 7,8white
	switch (choice)
	{
	case 1:
		Qeustion_options(currentIndex);
		break;
	case 2:
		Send_options(currentIndex);
		break;
	case 3:
	{
		Read_options(currentIndex);
		break;
	}
	case 4:
	{
		system("cls");
		ViewAllContacts(currentIndex);
		break;
	}
	case 5:
	{
		system("cls");
		Add_user_contacts(currentIndex);
		break;
	}
	case 6:
	{
		system("cls");
		RemoveContact(currentIndex);
		break;
	}
	case 7:
	{
		system("cls");
		start();
		break;
	}
	default:
	{
		system("cls");
		cout << "\n\t\t\t\t\t__________wrong choice, please try again ______________\n\n";
		UserOptions(currentIndex);
	}
	}
}
			//ok//gehad
void site::Qeustion_options(int currentIndex)
{
	system("cls");
	int choice;
	SetConsoleTextAttribute(h, 15);// 7,8white
	cout << "\n\t\t\t\t\t\tPress 1 to Send a Qeustion,";
	cout << "\n\t\t\t\t\t\tPress 2 to Remove the last Sent Qeustion.";
	cout << "\n\t\t\t\t\t\tPress 3 to View all your Qeustion.\n";
	cout << "\t\t\t\t\t\tEnter your chioce:  ";
	cin >> choice;
	SetConsoleTextAttribute(h, 7);// 7,8white
	switch (choice)
	{
	case 1:
		Send_Qestion(currentIndex);
		break;
	case 2:
		Undo_qeustion(currentIndex);
		break;
	case 3:
		ViewAllMyQeustion(currentIndex);
		break;
	default:
	{
		system("cls");
		cout << "\n\t\t\t\t\t__________wrong chioce, please try again ______________\n\n";
		UserOptions(currentIndex);
	}
	}
}
			//ok//gehad
void site::Send_options(int currentIndex)
{
	system("cls");
	int choice;
	cout << "\n\t\t\t\t\t\tPress 1 to View all Qeustion,";
	cout << "\n\t\t\t\t\t\tPress 2 to Send a Message,";
	cout << "\n\t\t\t\t\t\tPress 3 to View all Sent Message,";
	cout << "\n\t\t\t\t\t\tPress 4 to Remove the last Sent Message.\n";
	cout << "\t\t\t\t\t\tEnter your chioce:  ";
	cin >> choice;
	switch (choice)
	{
	case 1:
		ViewAllrecipientQeustion(currentIndex); // call send_message.
		break;
	case 2:
		send_message(currentIndex);
		break;
	case 3:
		ViewAllSentMessage(currentIndex);
		break;
	case 4:
		Undo_message(currentIndex);
		break;

	default:
	{
		system("cls");
		cout << "\n\t\t\t\t\t__________wrong chioce, please try again ______________\n\n";
		UserOptions(currentIndex);
	}
	}
}
			//ok//habiba mohammed
void site::RemoveContact(int currentIndex)
{
	system("cls");
	if (User[currentIndex - 1].contact.size() == 0)
	{
		cout << "\n\t\t\t\tyou do not have any contacts\n\t\t\t\t";
	}
	else 
	{
		int cnt_id;
		cout << "\n\t\t\t\tEnter the contact ID that you want to remove: ";
		cin>> cnt_id;

		bool found_id = false;

		for (auto it = User[currentIndex - 1].contact.begin(); it != User[currentIndex - 1].contact.end();it++) 
		{
			if (it->second == cnt_id)
			{
				it=User[currentIndex - 1].contact.erase(it);
				found_id = true;
			}
			
		}
		if (found_id)
		{
			cout << "\n\t\t\t\tContact has been removed successfully\n\t\t\t\t";
		}
		else
		{
			cout << "\n\t\t\t\tinvalid contact id\n\t\t\t\t";
		}
	}

	system("pause");
	UserOptions(currentIndex);
}
			//ok//gehad
void site::Read_options(int currentIndex)
{
	system("cls");
	int choice;
	cout << "\n\t\t\t\t\t\tPress 1 to View all received message,";
	cout << "\n\t\t\t\t\t\tPress 2 to Put the message in Favorites,";
	cout << "\n\t\t\t\t\t\tPress 3 to Remove the oldest message from Favorites,";
	cout << "\n\t\t\t\t\t\tPress 4 to View all Favorites message.\n";
	cout << "\t\t\t\t\t\tEnter your chioce:  ";
	cin >> choice;
	switch (choice)
	{
	case 1:
		ViewAllrecipientMessage(currentIndex);
		break;
	case 2:
		PutFavoritMessage(currentIndex);
		break;
	case 3:
		RemoveFavoritMessage(currentIndex);
		break;
	case 4:
		ViewAllfavoritMessage(currentIndex);
		break;
	default:
	{
		system("cls");
		cout << "\n\t\t\t\t\t__________wrong chioce, please try again ______________\n\n";
		UserOptions(currentIndex);
	}
	}
}
			//ok//gehad
void site::Send_Qestion(int currentIndex)
{
	system("cls");
	string ques;
	cout << "\n\t\t\t\t\t\tEnter the Qeustion you want to send: ";
	cin.ignore();
	getline(cin, ques);

	User[currentIndex - 1].sentquestions.push_back(ques);
	Sentmessages m1;
	m1.message = ques;
	m1.id_messsage = User[currentIndex - 1].sentquestions.size() - 1;

	m1.id_user = currentIndex;
	if (User[currentIndex - 1].contact.size() == 0)
	{
		SetConsoleTextAttribute(h, 6);
		cout << "\n\t\t\t\t\t\tyou do not have any recipient\n\n\t\t\t\t\t\t";
		SetConsoleTextAttribute(h, 7);
	}
	else {
		int userId;
		for (auto it = User[currentIndex - 1].contact.begin(); it != User[currentIndex - 1].contact.end();it++) {
			userId = it->second;
			User[userId - 1].recipientquestions.push_back(m1);
		}
		cout << "\n\t\t\t\t\t\tyour question has sent successfully\n\t\t\t\t\t\t";
	}
	system("pause");
	UserOptions(currentIndex);
}
			//ok//gehad
void site::Undo_qeustion(int currentIndex)
{
	User[currentIndex - 1].sentquestions.pop_back();
	int userId;
	for (auto it = User[currentIndex - 1].contact.begin(); it != User[currentIndex - 1].contact.end(); it++) {
		userId = it->second;
		for (int j = User[userId - 1].recipientquestions.size() - 1; j >= 0; j--) {
			if (User[userId - 1].recipientquestions[j].id_user == currentIndex)
				User[userId - 1].recipientquestions.erase(User[userId - 1].recipientquestions.begin() + j);
		}
	}
	cout << "\n\t\t\t\t\t\tyour question has deleted successfully\n\t\t\t\t\t\t";
	system("pause");
	UserOptions(currentIndex);
}
			//ok//nada
void site::ViewAllMyQeustion(int currentIndex)
{
	system("cls");
	SetConsoleTextAttribute(h, 12);
	cout << "\n\t\t\t\t\t\t* * * * * * * * \n";
	cout << "\t\t\t\t\t\t*Your Question*\n";
	cout << "\t\t\t\t\t\t* * * * * * * *\n";
	SetConsoleTextAttribute(h, 7);
	cout << "\n\t\t\t\t\t\t______________________________________________________________\n";
	for (int i = User[currentIndex - 1].sentquestions.size() - 1; i >= 0; i--) {
		cout << "\t\t\t\t\t\t| " << User[currentIndex - 1].sentquestions[i] << " |\n";//
	}
	cout << "\t\t\t\t\t\t______________________________________________________________\n\t\t\t\t\t\t";
	system("pause");
	UserOptions(currentIndex);
}
			//ok//hasnaa
void site::PutFavoritMessage(int currentIndex)
{
	system("cls");
	bool favo = false;
	SetConsoleTextAttribute(h, 12);
	cout << "\n\t\t\t\t\t\t* * * * * * * * \n";
	cout << "\t\t\t\t\t\t* the message *\n";
	cout << "\t\t\t\t\t\t* * * * * * * *\n";
	SetConsoleTextAttribute(h, 7);
	if (User[currentIndex - 1].recipientmessage.size() == 0)
		cout << "\n\t\t\t\tSorry you do not have any message to put in favorite list\n\t\t\t\t";
	else {
		cout << "\n\t\t\t\t\t______________________\n";
		int idmess;
		for (int i = 0; i < User[currentIndex - 1].recipientmessage.size(); i++) {
			cout << "\t\t\t\t\t" << i + 1 << "- " << User[currentIndex - 1].recipientmessage[i].message << "\n";
		}
		cout << "\t\t\t\t\t______________________\n\t\t\t\t\t";
		cout << "\n\t\t\t\t\tEnter the number of message you want to add in favorite: ";
		cin >> idmess;
		if (idmess <= 0 && idmess > User[currentIndex - 1].recipientmessage.size())
		{
			cout << "\n\t\t\t\t\tThis number is wrong please enter correct number: ";
			cin >> idmess;
		}
		for (auto i = User[currentIndex - 1].favoritmessage.begin(); i != User[currentIndex - 1].favoritmessage.end(); i++)
		{
			if (idmess == i->first) {
				cout << "\n\t\t\t\t\tThis message was added already\n\t\t\t\t\t";
				favo = true;
			}
		}
		idmess--;
		if (favo == false)
		{
			for (int i = 0; i < User[currentIndex - 1].recipientmessage.size(); i++) {

				if (idmess == i) {
					User[currentIndex - 1].favoritmessage.emplace(i + 1, User[currentIndex - 1].recipientmessage[i].message);
				}
			}
			cout << "\n\t\t\t\t\tthe message added successfully\n\t\t\t\t\t";
		}
	}
	system("pause");
	UserOptions(currentIndex);
}
			//ok//hasnaa
void site::RemoveFavoritMessage(int currentIndex)
{
	system("cls");
	SetConsoleTextAttribute(h, 12);
	cout << "\n\t\t\t\t\t\t* * * * * * * * \n";
	cout << "\t\t\t\t\t\t* the message *\n";
	cout << "\t\t\t\t\t\t* * * * * * * *\n";
	SetConsoleTextAttribute(h, 7);
	cout << "\n\t\t\t\t ______________________\n";
	if (User[currentIndex - 1].favoritmessage.empty() == true)
		cout << "\n\t\t\t\t YOU DO NOT HAVE ANY FAVORATE MESSAGE \n\t\t\t\t";
	else {
		User[currentIndex - 1].favoritmessage.erase(User[currentIndex - 1].favoritmessage.begin());
		cout << "\n\t\t\t\t MESSAGE DELETED \n\t\t\t\t\t";
	}
	cout << "\n\t\t\t\t ______________________\n\t\t\t\t";
	system("pause");
	UserOptions(currentIndex);
}
			//ok//hasnaa
void site::ViewAllfavoritMessage(int currentIndex)
{
	system("cls");
	SetConsoleTextAttribute(h, 12);
	cout << "\n\t\t\t\t\t\t* * * * * * * * \n";
	cout << "\t\t\t\t\t\t* the message *\n";
	cout << "\t\t\t\t\t\t* * * * * * * *\n";
	SetConsoleTextAttribute(h, 7);
	if (User[currentIndex - 1].favoritmessage.empty() == true)
		cout << "\n\t\t\t\t YOU DO NOT HAVE ANY FAVORATE MESSAGE \n\t\t\t\t";
	else
	{
		cout << "\n\t\t\t\t______________________\n";
		cout << "\n\t\t\t\t YOUR FAVORATE MESSAGR LIST \n";

		for (auto i = User[currentIndex - 1].favoritmessage.begin(); i != User[currentIndex - 1].favoritmessage.end(); i++)
		{
			cout << "\n\t\t\t\t" << i->first << "- " << i->second << '\n';
		}
		cout << "\n\t\t\t\t______________________\n\t\t\t\t ";
	}

	system("pause");
	UserOptions(currentIndex);
}
			//ok//habiba _ahmed
void site::send_message(int currentIndex)
{
	system("cls");
	int id_message, numofmes, id;
	bool is_found = false;/////cout enter the id person
	cout << "\n\t\t\t\t\tEnter the id of user you want to answer his question: ";
	cin >> id;
	for (int i = 0; i < User.size(); i++) {
		for (int k = 0; k < User[i].recipientquestions.size(); k++) {
			if (id == User[i].recipientquestions[k].id_user)
			{
				is_found = true;
				break;
			}
		}
	}
	if (is_found)
	{
		SetConsoleTextAttribute(h, 12);
		cout << "\n\t\t\t\t\t\t* * * * * * * * \n";
		cout << "\t\t\t\t\t\t* the Question*\n";
		cout << "\t\t\t\t\t\t* * * * * * * *\n";
		SetConsoleTextAttribute(h, 7);
		cout << "\n\t\t\t\t\t______________________________________________________________\n";
		int i = 0;
		while (i < User[currentIndex - 1].recipientquestions.size())
		{
			if (User[currentIndex - 1].recipientquestions[i].id_user == id) //check if user found in send him message
			{
				id_message = User[currentIndex - 1].recipientquestions[i].id_messsage;
				cout << "\t\t\t\t\t" << id_message + 1 << "- " << User[currentIndex - 1].recipientquestions[i].message << "\n"; //Display Questins & their ids.
			}
			i++;
		}
		cout << "\t\t\t\t\t______________________________________________________________";
		Sentmessages s;
		cout << "\n\t\t\t\t\tEnter Id Question You Want To Answer On It From This List: ";
		cin >> s.id_messsage;
		s.id_messsage--;
		bool isfound_id_messsage = false;
		for (int i = 0; i < User[currentIndex - 1].recipientquestions.size(); i++)
		{
			if (s.id_messsage == User[currentIndex - 1].recipientquestions[i].id_messsage)
			{
				isfound_id_messsage = true;
				break;
			}
		}
		if (isfound_id_messsage)
		{
			cout << "\n\t\t\t\t\tEnter Message You Want To Send It:\n\t\t\t\t\t";
			cin.ignore();
			getline(cin, s.message);
			s.id_user = currentIndex;
			User[id - 1].recipientmessage.push_back(s);//answer,id_user_answer,id_question.
			User[currentIndex - 1].sentmessage.push_back(s.message);//answer.

			for (auto it = User[id].contact.begin(); it != User[id].contact.end(); it++) {
				if (it->second == currentIndex) {
					numofmes = it->first + 1;
					User[id - 1].contact.erase(it);
					User[id - 1].contact.emplace(numofmes, currentIndex);
					break;
				}
			}
			cout << "\n\t\t\t\t\tYour Message has sent Successfully\n\t\t\t\t\t";
		}
		else
		{
			cout << "\n\t\t\t\t\tInvalid Id_question,Please choose From the list will show!!!\n\t\t\t\t\t";
		}
		system("pause");
		UserOptions(currentIndex);
	}
	else
	{
		cout << "\n\t\t\t\t\tUser Is n't Found.\n\t\t\t\t\t";
		system("pause");
		Send_options(currentIndex);
	}
}
			//ok//habiba _ahmed
void site::Undo_message(int currentIndex)
{
	system("cls");
	int current_Index = 0;
	int id; int numofmes;
	bool is_found = false, is_found_person = false;
	if (User[currentIndex - 1].sentmessage.size() == 0)
	{
		cout << "\n\t\t\t\tThere Are No Messages To Undo.\n\t\t\t\t";
		system("pause");
		Send_options(currentIndex);
	}
	else if (User[currentIndex - 1].sentmessage.size() != 0)
	{
		cout << "\n\t\t\t\tEnter Id You Want To Delete The Last Sentmessage .\n\t\t\t\t";
		cin >> id;
		for (int k = 0; k < User[currentIndex - 1].recipientquestions.size(); k++) {
			if (id == User[currentIndex - 1].recipientquestions[k].id_user) {
				current_Index = 1;
				break;
			}
		}
		if (current_Index) {
			for (int j = User[id - 1].recipientmessage.size() - 1; j >= 0; j--)
			{
				if ((currentIndex == User[id - 1].recipientmessage[j].id_user) && (User[currentIndex - 1].sentmessage.back() == User[id - 1].recipientmessage[j].message))
				{
					User[id - 1].recipientmessage.erase(User[id - 1].recipientmessage.begin() + j);
					cout << "\n\t\t\t\tThis Message Will Deleted.\n\t\t\t\t" << User[currentIndex - 1].sentmessage.back();
					User[currentIndex - 1].sentmessage.pop_back();
					for (auto it = User[id - 1].contact.begin(); it != User[id - 1].contact.end(); it++) {
						if (it->second == currentIndex) {
							numofmes = it->first - 1;
							User[id - 1].contact.erase(it);
							User[id - 1].contact.emplace(numofmes, currentIndex);
							break;
						}
					}
					cout << "\n\t\t\t\tThis message deleted Successfully\n\t\t\t\t";
					is_found = true;
					break;
				}
			}
		}
		else
		{
			cout << "\n\t\t\t\tThis Person Not found \n\t\t\t\t";
		}
		if (!is_found&& current_Index)
		{
			cout << "\n\t\t\t\tThis Person Not has your message\n\t\t\t\t";
		}
		system("pause");
		UserOptions(currentIndex);
	}
}
			//ok//toqa
void site::ViewAllContacts(int currentIndex)
{
	system("cls");

	if (User[currentIndex - 1].contact.size() == 0)
	{
		cout << "\n\t\t\t\t You do not have contacts\n\t\t\t\t ";
	}

	else
	{
		SetConsoleTextAttribute(h, 12);
		cout << "\n\t\t\t\t\t\t* * * * * * * * \n";
		cout << "\t\t\t\t\t\t* Your Contacts *\n";
		cout << "\t\t\t\t\t\t* * * * * * * *\n";
		SetConsoleTextAttribute(h, 7);
		cout << "\n\t\t\t\t______________________________________________________________\n";

		for (auto it = User[currentIndex - 1].contact.rbegin(); it != User[currentIndex - 1].contact.rend(); ++it)
		{
			cout << "\t\t\t\tnumofmessage: " << it->first << " , " << "id: " << it->second << endl;
		}
		cout << "\t\t\t\t______________________________________________________________\n\t\t\t\t";
	}
	system("pause");
	UserOptions(currentIndex);
}
			//ok//marim
void site::Add_user_contacts(int currentIndex)
{   /*
		check if this user sends a message,
		then add him to the list of contacts else throw an exception or print a message.
	*/
	bool found_recipient_mes = false, found_recipient_que = false, found_contact = false;
	int senderid;
	cout << "\n\t\t\t\tEnter id you want to add him in your contact: ";
	cin >> senderid;
	
	for (int i = 0; i < User[currentIndex - 1].recipientmessage.size(); i++) {
		if (User[currentIndex - 1].recipientmessage[i].id_user == senderid)
		{
			found_recipient_mes = true;
			found_contact = SearchAboutContact(currentIndex, senderid);
			break;
		}
	}
	for (int i = 0; i < User[currentIndex - 1].recipientquestions.size(); i++) {
		if  (User[currentIndex - 1].recipientquestions[i].id_user == senderid)
		{
			found_recipient_que = true;
			found_contact = SearchAboutContact(currentIndex, senderid);
			break;
		}
	}
	if ((found_recipient_mes && found_contact)|| (found_recipient_que && found_contact)) {
		cout << "\n\t\t\t\tthis user is existed in your contact\n\t\t\t\t";
	}
	else if (found_recipient_mes && (!found_contact)) {
		User[currentIndex - 1].contact.insert(make_pair(1, senderid));
		cout << "\n\t\t\t\tthis user added successfully\n\t\t\t\t";
	}
	else if (found_recipient_que && (!found_contact)) {
		User[currentIndex - 1].contact.insert(make_pair(0, senderid));
		cout << "\n\t\t\t\tthis user added successfully\n\t\t\t\t";
	}
	else {
		cout << "\n\t\t\t\tthis user did not send you a message\n\t\t\t\t";
	}
	system("pause");
	UserOptions(currentIndex);
}
			//ok//marim
bool site::SearchAboutContact(int currentIndex,int sender_id)
{
	for (auto it = User[currentIndex - 1].contact.begin(); it != User[currentIndex - 1].contact.end(); it++) {
		if (it->second==sender_id)
		{
			return true;
		}
	}
	return false;
}
			//ok//gehad
void site::ViewAllrecipientMessage(int currentIndex)
{
	system("cls");
	int userId;
	cout << "\n\t\t\t\t\t Enter the ID that you want to view his message: ";
	cin >> userId;
	userId;
	bool foundUser = false;
	for (auto it = User[currentIndex - 1].contact.begin(); it != User[currentIndex - 1].contact.end(); it++) {
		if (it->second = userId)
			foundUser = true;
	}
	if(!foundUser)
	{
		cout << "\n\t\t\t\t\tthis id isn't exist in your contact\n\t\t\t\t\t";
	}
	else 
	{
		system("cls");
		SetConsoleTextAttribute(h, 12);
		cout << "\n\t\t\t\t\t\t* * * * * * * * \n";
		cout << "\t\t\t\t\t\t* the message *\n";
		cout << "\t\t\t\t\t\t* * * * * * * *\n";
		SetConsoleTextAttribute(h, 7);
		cout << "\n\t\t\t\t\t______________________________________________________________\n";
		int idQues;
		for (int i = 0; i < User[currentIndex - 1].recipientmessage.size(); i++) {
			if (User[currentIndex - 1].recipientmessage[i].id_user == userId) {
				idQues = User[currentIndex - 1].recipientmessage[i].id_messsage;
				cout << "\t\t\t\t\t| " << User[currentIndex - 1].sentquestions[idQues] << " |\n";
				cout << "\t\t\t\t\t| " << userId << " | " << User[currentIndex - 1].recipientmessage[i].message << " |\n";
			}
		}
		cout << "\t\t\t\t\t______________________________________________________________\n\t\t\t\t\t";
	}
	system("pause");
	UserOptions(currentIndex);
}
			//ok//toqa
void site::ViewAllSentMessage(int currentIndex)
{
	system("cls");
	SetConsoleTextAttribute(h, 12);
	system("cls");
	cout << "\n\t\t\t\t\t\t* * * * * * * * *  * \n";
	cout << "\t\t\t\t\t\t*Your Sent Messages*\n";
	cout << "\t\t\t\t\t\t* * * * * * * * *  *\n";
	SetConsoleTextAttribute(h, 7);

	if (User[currentIndex - 1].sentmessage.size() == 0)
	{
		cout << "\t\t\t\t\t\t " << "You Did not send any messages yet" << "\t\t\t\t\t\t ";
	}
	else
	{
		cout << "\n\t\t\t\t\t\t______________________________________________________________\n";
		for (auto it = User[currentIndex - 1].sentmessage.rbegin(); it != User[currentIndex - 1].sentmessage.rend(); it++)
		{
			cout << "\t\t\t\t\t\t| " << *it << " |\n";;
		}
	}
	cout << "\t\t\t\t\t\t______________________________________________________________\n\t\t\t\t\t\t";
	system("pause");
	UserOptions(currentIndex);
}
			//ok//habiba mohammed
void site::ViewAllrecipientQeustion(int currentIndex)  //US
{
	system("cls");
	SetConsoleTextAttribute(h, 12);
	system("cls");
	cout << "\n\t\t\t\t\t\t* * * * * * * * * * * * *\n";
	cout << "\t\t\t\t\t\t*Your recipient Qeustion*\n";
	cout << "\t\t\t\t\t\t* * * * * * * * * * * * *\n";
	SetConsoleTextAttribute(h, 7);
	if (User[currentIndex - 1].recipientquestions.size()==0)
	{
		cout << "\n\t\t\t\t\t\tyou don't have any recipient questions\n\t\t\t\t\t\t";
		system("pause");
		UserOptions(currentIndex);
	}
	else
	{
		cout << "\n\t\t\t\t\t______________________________________________________________\n";
		for (int i = User[currentIndex - 1].recipientquestions.size() - 1; i >= 0; i--)
		{
			cout << "\t\t\t\t\t " << User[currentIndex - 1].recipientquestions[i].id_user << "\n";
			cout << "\t\t\t\t\t| " << User[currentIndex - 1].recipientquestions[i].message << " |\n";
		}
		cout << "\t\t\t\t\t______________________________________________________________";
		cout << "\n\t\t\t\t\tDo you want to answer any of these questions ?(yes/no): ";
		string cho;
		cin >> cho;
		if (cho == "yes" || cho == "Yes" || cho == "YES")
		{
			send_message(currentIndex);
		}
		else
		{
			UserOptions(currentIndex);
		}
	}
}
			//ok
site::~site()
{
	User.clear();
}
