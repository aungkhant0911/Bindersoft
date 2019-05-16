# About

A multi-file combiner application written in VB.Net a while ago. Been in a situation where I wished I could treat as many programs or files of any type as a single one. Not only that but this program also allows one to keep files encrypted, granting portability and not have to worry about the threat of antivirus software.

The sequential binding nature of this utility allows for procedural execution of files / script (if they are of executable types).



# Requirements

.Net Runtime 3.5
Visual Basic.Net 2008 or later is required to edit source.



# How it works?
  - Builder is used to bind files. The following diagram provides the user interaction and what it takes to get a single executable file, called Stub.
  
  ![Capturing](/builder_workflow.jpg)
  
  
  - Once the Stub has been created, it can then be executed, upon which one can get back all the original files. Note that the files will be written to 1 of the 3 places chosen when Stub is built.
  
  ![Capturing](/stub_workflow.jpg)



#
