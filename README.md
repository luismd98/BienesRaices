Prueba | Frontend - Backend
Propuesta de solución para compañía de bienes raíces – Luis Martínez

Instrucciones para ejecutar el proyecto: 

1.	Instalación de la base de datos:
1.1.	En SQL Server Management Studio, click derecho en “Databases” > New Database
1.2.	Ingrese el nombre de la base de datos: “BienesRaices” y click en OK

 ![image](https://user-images.githubusercontent.com/54687614/157125140-95a1a4d8-049a-4d41-b658-e366d2deac11.png)

 
1.3.	Expandir la carpeta Databases, click derecho en la base de datos recién creada y seguir la ruta:  Task (tarea) > Restore (restaurar) > Database (base de datos
1.4.	Seguir los pasos en orden de las siguientes capturas

 ![image](https://user-images.githubusercontent.com/54687614/157125147-558e7ff5-27e8-42f8-a3ef-90bd576bebdc.png)


1.5.	Buscar y seleccionar el archivo descargado: BienesRaices.bk
 ![image](https://user-images.githubusercontent.com/54687614/157125159-8e9f3bbc-5f8b-443d-98a2-7079cd643d3d.png)


1.6.	Click en Ok, seguir los pasos de las siguientes capturas en orden:
![image](https://user-images.githubusercontent.com/54687614/157125169-6a749ec9-9349-47fe-b9b6-d59b5f802d89.png)

![image](https://user-images.githubusercontent.com/54687614/157125182-90134a12-9686-423a-abf7-a785994e714b.png)

![image](https://user-images.githubusercontent.com/54687614/157125201-f7d62799-107c-4ac1-9cad-f4031c124b4b.png)


1.7.	Repetir Todo para la base de datos: BienesRaicesTest (BienesRaicesTest.bk)

2.	Ejecutar Web -API
2.1.	Ejecutar con Visual Studio el proyecto ApiRaices (ApiRaices.sln)
2.2.	En caso de que la instancia de SQL Express tenga un nombre diferente a “SQLExpress”, ir al archivo “appsettings.json” y cambiar la parte mencionada anteriormente por el nombre de instancia instalado en el equipo a ejecutar
2.3.	Presionar el botón para correr el programa
 ![image](https://user-images.githubusercontent.com/54687614/157125260-8a0e8acb-b738-4ee6-a111-c2313291f1e5.png)


3.	Ejecutar Frontend (Angular)
3.1.	Luego de ejecutar el Web-API, revisar el puerto, si se encuentra en uno diferente a “44314”, se debe ingresar al archivo: “BienesRaices\FrontRaices\src\app\shared.service.ts” y poner el puerto correspondiente en la línea 12 y 13


4.	Correr pruebas de Web-API
4.1.	Con el proyecto abierto (no en ejecución), ira NUnitTesting > OwnerTest.cs
4.2.	En el panel “Text Explorer”, presionar el botón de ejecutar todos
4.3.	Pruebas disponibles: 
•	Owner Controller (CRUD) | Se revisa el código de retorno:
  o	Acción satisfactoria
  o	Error en la base de datos 
  o	Validación de información (ejemplo: Nombre en blanco, Birthday fuera de rango, etc)
Si el panel está oculto, usar el buscador para activarlo:
 
![image](https://user-images.githubusercontent.com/54687614/157125276-fee8c46e-2dda-4b3e-bc1a-647cbb87db22.png)



Observaciones:

Por cuestión de tiempo, no se alcanza a terminar:
 • Documentación del código (interno)
 • Pruebas para todos los controladores (Solo OwnerController)
