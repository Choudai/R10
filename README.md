# R10 - Ransomware
**The Fastest Most Lightweight Ransomware Targeting Windows 10 @Choudai**
> This project is purely academic, use at your own risk. I do not encourage in any way the use of this software illegally or to attack targets without their previous authorization.

### What is Ransomware?
Ransomware is a type of malicious software designed to block access to a computer system until a sum of money is paid.

### Summary
This project aims to build a functional ransomware for educational purposes, written in C#. In short, it encrypts your user files silently using XOR-ISSAC a strong encryption algorithm, and then drops a ransom note.

### Testing
To start create a folder on your Desktop named 'TEST' and put in any files you want destroyed.
Compile and run the program using Visual Studio 2017 and make sure the target C# framework is version 4.6 (version 2.0 is also possible).
The encryption algorithm uses streams, so it should be able to handle files larger than 4GB.
