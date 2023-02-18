# AtmWebAPI
The ATM RESTful Web API Service 

Project Specifications

The ATM RESTfulWeb API ServiceThe candidate is required to implement an application mimicking a bank’s ATM. 
To keep things as simple as possible let’s consider a single user account and a single currency, euros, EUR. 
The user should be able to perform the below actions:

•Deposit money to an account
The ATM will only accept 10(BankNoteType:”Ten”), 20 (BankNoteType:”Twenty”)and 50 (BankNoteType:”Fifty”)
euro notes to be depositedand each deposit operation will support a single type of 
bank notes. So, if for example the user requires to deposit 130 euros into the account, which are in 2x50, 1x20 and 1x10 bank notes, 
3 deposit operations must be performed, one for each bank note type:
One for 100 euros of BankNoteType: “Fifty”
One for 20 euros of BankNoteType: “Twenty”
One for 10 euros of BankNoteType: “Ten”
Upon successful operation the API must return a success HTTP 
response code (200) in the header with a JSON containing the new Balance of the accountgrouped by the available bank notesand the 
timestamp of the transaction.
For example:

{
“Timestamp”:”2022-11-01T23:03:58”,
“Available”:170,
“Fifty”:2,
“Twenty”:2,
“Ten”:3
}
•Withdrawmoney 
from an accountThe ATM should return the amount requested by the user using bank notes from the higher value to the smaller. Also,the ATM 
should only use 50-eurobank notes only if the amount requested exceeds or is equal to 100 euros.If the amount of a required bank note type is
not sufficient, then the logic should move on to the smaller value bank note types.
For example:
if the user requests to make a withdrawal of 
50 euros and there are available funds in the account, the ATM should return:2x20,1x10bank notes

if the user requeststo make a withdrawal of 
60 euros and thereare available funds in the account, the ATM should return 3x20 bank notes

if the user requests to make a withdrawal of 130 
euros and there are available funds in the account, the ATM should return 2x50,1x20, 1x10 bank notes

if the user requests to make awithdrawal 
of 60 euros but only one Twenty note exists, the ATM should return 1x20, 4x10 bank notes

if the user requests to make a withdrawal of 130 
euros but only one Fifty note exists, and no Twenty notes, the ATM should return 1x50, 8x10 bank notes

Upon successful operation the API must return a success HTTP response code (200) in the header with a JSON containing the new Balance of the account grouped by the available bank notesand the timestamp of the transaction.
For example:
{
“Timestamp”:”2022-11-02T19:43:26”,
“Available”:120,
“Fifty”:2,
“Twenty”:1
}

If the available bank notesor the available funds cannot cover the amount requested,
then the response should be an HTTP BadRequest code (400)
with the message “Insufficient Funds.”

Return the available funds, i.e., the balance,of theaccountThis should be a timestampedinteger of the available funds in the account.
Upon successful operation the API must return a success HTTP response code (200) in the header with a JSON containing the
new Balance of the account grouped by the available bank notes and the timestamp of the transaction. 

For example:
{
“Timestamp”:”2022-11-02T20:52:18”,
“Available”:120,
“Fifty”:2,
“Twenty”:1
}
Get a full transactions history, i.e., a statement, of the accountReturn a full transactions history. 
Each transaction recorded should have the following information/properties:
Transaction ID: A GUID identifying the transaction
UTC TimestampinISO 8601date timeformat (“yyyy-MM-ddTHH:mm:ss”)E.g., 2022-10-25T18:53:46
Transaction Type: “deposit”, “withdraw”
Amount
BankNoteType: Only for deposit transactions, indicating the bank note type of a deposit. 
This should be nullfor transactions of type “withdraw

Upon successful operation the API must return a success HTTP response code (200) in the header with a JSON containing an array of 
all recorded transactions and the current available balance of the account.
For example:
{
"TransactionsHistory": [
{
"Id": "2192aba8b5794e1294b15e8f9869eefe",
"Timestamp": "2022-11-02T20:52:18",
"TrxType": "Deposit",
"Amount": 50,
"BankNotesType": "Ten"
},
{
"Id": "51a124ae729c4ac59bc67f4748545417",
"Timestamp": "2022-11-02T20:52:18",
"TrxType": "Deposit",
"Amount": 100,
"BankNotesType": "Ten"
},
{
"Id": "8edfa6dfc4524f8a84de550810b0087b",
"Timestamp": "2022-11-02T21:32:12",
"TrxType": "Deposit",
"Amount": 120,
"BankNotesType": "Twenty"
},
{
"Id": "e2db5eb314ff4798946ca8bbe0accf70",
"Timestamp": "2022-11-02T21:34:34",
"TrxType": "Withdraw",
"Amount": 50,
"BankNotesType": "Fifty"
},
{
"Id": "b492514911bb421480f0c9217788ac79",
"Timestamp": "2022-11-02T22:00:52",
"TrxType": "Withdraw",
"Amount": 10,
"BankNotesType": "Ten"
},
{
"Id": "a007da14e71a4018a576c9c43b7217a6",
"Timestamp": "2022-11-02T22:28:00",
"TrxType": "Deposit",
"Amount": 50,
"BankNotesType": "Fifty"
},
{
"Id": "b3f4e0d0e85d42ada629d8cb0621aa27",
"Timestamp": "2022-11-02T23:43:36",
"TrxType": "Deposit",
"Amount": 120,
"BankNotesType": "Twenty"
}
],
"Available": 380
}

The above operations,and the overall logic of the ATM,must be methods and actions to relevant controllers in a RESTful Web API Service,
which will be using an SQL Server Database for storage.All actions return the relevant HTTP response codein the header:
•200–OK for successful operations
•400 –BadRequest for unsuccessful ones
All actions return a JSONresponse with the relevant information as described above
