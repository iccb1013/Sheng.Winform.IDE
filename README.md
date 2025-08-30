# Sheng.Winform.IDE

🙋‍♂️ https://me.shendesk.com
📨 cao.silhouette@gmail.com

Please visit the original code repository for the latest updates: [https://github.com/iccb1013/Sheng.Winform.IDE](https://github.com/iccb1013/Sheng.Winform.IDE).

This project is open source under the MIT license. You are free to use it as you wish, but please retain the copyright notice and my website link in the source code and the "About" screen of your product. Thank you.

This project was developed during my spare time between 2009 and 2011. The initial idea was simple: create a tool that could generate applications directly by dragging and dropping, without writing code. All the business operations that I could think of were encapsulated, and these operations were organized and executed through configuration.  
The core functionality of the project has been mostly implemented, but after 2012, I stopped developing in this area. I’m now sharing it here for communication and hope it will be useful to you.

> I'm sorry that I didn't prepare a multilingual version at the time. If you have any questions about the code, please feel free to contact me, and I will be happy to assist you.

![b60aad02-98d5-4219-b70d-df1fb95863aa](https://github.com/user-attachments/assets/e8d1fa17-72a1-4b99-83b0-ea963dd08f86)

![8d156e63-2a67-498f-a6c2-5b6de3da846c](https://github.com/user-attachments/assets/67946729-65ff-4bd5-8494-17e2debfb6b4)

![28b49aa4-2c29-4fb1-9419-b29214a831de](https://github.com/user-attachments/assets/07076c64-d572-482f-8459-456c1612bc7d)


Let me briefly explain the design concept of the IDE, with the main design goals as follows:

- **Like Visual Studio**:  
  There is a visual environment, where interfaces can be created by simply dragging and dropping.
  
- **Modular Design**:  
  All functional modules are independent, decoupled, and exist as plugins within the main application (host).

- **No Coding Required, Business Configured via Interface or Wizard**:  
  To add a button and make it perform a specific action on click, just drag the button onto the form, then configure the event sequence and the corresponding actions.

- **Abstract and Encapsulate the Concept of Events**:  
  For example, the "Save Data" event. You configure the source of the data, such as form data or system data, then specify the target for saving, like a data entity. The event is then added to an event sequence (e.g., the button's click event). When the project is executed, this logic is processed when the button is clicked.

- **Flexible Data Operations**:  
  In addition to basic wizard-based configurations, special needs must also be met, such as supporting custom SQL statements. How does a custom SQL statement interact with the data source or target? I designed a simple expression format, like `UPDATE FROM [User] SET [Name] = {FormElement.txtName} WHERE [Id] = {System.UserId}`.

- **Interaction with Database Tables**:  
  Data operations are abstracted as "data entities," which the user defines within the IDE, similar to how SQL Server operates. Once the data entities are defined, they can be used in the IDE to abstract database and table operations. When packaging the project, the IDE can generate multiple databases, such as SQL Server or MySQL, based on the defined data entities.

- **Resource File Management**:  
  External resources are often referenced in a project. The IDE includes a dedicated resource manager to manage and bundle these resources. Resources are referenced via the resource manager during UI design and are packaged into a zip file during the build process.

- **Static Compilation Checks Before Packaging**:  
  Similar to how Visual Studio warns about errors during compilation, this IDE provides real-time checks and warnings. If referenced data entities are deleted, data items are missing, resource files are absent, or there are issues in event configurations, the IDE will point out these specific errors during project packaging.

- **Support for Embedded Scripts**:  
  Custom scripts can be added to event sequences. The IDE supports dynamic parsing or execution of specific script languages at runtime. This feature was designed but not fully developed.

- **Support for Plugins**:  
  The IDE supports plugins at the IDE level, similar to the plugin mechanisms in Visual Studio or Eclipse. I used .NET pipeline technology (which was quite niche) to implement a related demo, but it was never integrated into the IDE.

- **Multi-Language Support in the IDE Interface**:  
  The IDE fully supports multiple languages. All text is stored in resources, but instead of directly using resource files, I strongly typed them for better management.
