#pragma once
#include<vector>
#include<list>
#include<iostream>
#include<unordered_map>
#include<map>
using namespace std;
class Sentmessages {
public:
	int id_user,id_messsage;
	string message;
	Sentmessages();
};
class user
{
	public:
	int id ;
	string username, password;
	multimap<int,int> contact;
	list<string> sentmessage;
	vector<Sentmessages> recipientmessage;
	unordered_map<int, string> favoritmessage;
	vector<string> sentquestions; // send quetion to ids which i have in my contacts
	vector<Sentmessages> recipientquestions;
	user();
	~user();
	user(int id, string username, string password);
};

