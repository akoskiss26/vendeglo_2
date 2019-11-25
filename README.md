# vendeglo_2
étterem project  Netacademia tanfolyam alapján

##1. feladat:
szeretnénk, hogy ne automatikusan hozza létra az adatbázist, hanem meghatározhassuk
	- mikor hozza létra
	- hova hozza létre
	- saját adatainkat is el tudjuk benne helyezni

Code First: elkészítem az alaklmazást, ami az adatokat használja, MAJD elkészül az adatmentés 
(az alkalmazás létrehozza az adatbázist)

Migration (Entity Framerwork Code First Migrations): eszköz, ami az adatmodell módosításokat kezeli
(Az Identity megcsinálta az adatbázist, mi meg szzeretnénk módosítani, saját adatokat belerakni)

A Code First Migration beüzemelése:
- előfeltétel az EntityFramework Nuget csomag megléte (Az ASP.NET MVC Identity telepítette, ezért ez megvan)
- package manager consol (PM) megnyitása
- PM> enable-migrations

Code First használata:
- az Identity egy saját adatmodellt csinált és használ
- saját adatbázishoz (ami a mi sajátunk) a Migration adatbázist módosít
- először az Identity modelljének kiírása egy migration step-be:
PM> add-migration 'Identity datamodel' (''-ben tetszőleges megnevezés lehet)
- ez létrehozta a Migration\nnn_Identity datamodel.cs állományt, meg még két technikai állományt
- fentit úgy hívjuk hogy adatbázis módosító lépés (migration step)
- migration step két fontos része az Up() és a Down() fgv (módosítás bejátszására és visszavonására)

- A MIgration step kiírása adatbázisba:
- PM> update-database  (ez nem fut le, mert az adatbázisállományt töröltük, így nem találja )	
	a webes alkalmazások paraméterezése a web.config állományban van
	Ebben "DefaultConnection" néven meg van adva az adatbázis helye
	a "DefaultConnection" a <connectionStrings>  ....  </connectionStrings> részben van, ezt a részt töröljük  (kiremeljük)
- PM> update-database (így már lefut)


Összefoglalva: Mit csináltunk eddig?
-----------------------------------
- kivarázsoltuk az alkalmazást (ASP.NET Webb App.  .NET framework)
- töröltük a létrejött adatbázist
- webconfig-ban kitöröltünk egy részt
- enable-migration
- add-migrations
- update-database

Fenti lépésekkel csináltunk egy új adatbázist (az eredeti adatbázist migráltuk és új adatbázisba raktuk)

Az adatbázis a saját gépen a Default SQL Instance -ra került, a neve pedig: DefaultConnection

## adjuk meg, hova kerüljön az adatbázis:

- készítünk egy kapcsolati beállítást a www.connectionstrings.com segítségével
	- megkeressük az SQL servert -> .NET Framework Data Provider for SQL Server connection strings
	-> Connection to a SQL Server instance

fentiek alapján a Web.config állományban a kiremelt részben  a connectionString paraméterbe beírjuk a:
- server nevét  [server=(localDB)\MSSQLlocalDB]
- az adatbázis nevét: [restaurantDB]
- módszer, ahogy a felhasználó bejelentkezik: [Trusted_Connection=True]

törljük a régi DefaultConnection adatbázisunkat

PM> update-database

létrejött az új adatbázis, az Object Explorerben látható, hogy megvan a restaurantDb adatbázis
és a táblák is megvannak


az update-database lefutása után a Migrations könyvtárban létrejött három állomány
ezek közül az egyik a nnnn_identity datamodel.cs (sic)
	ez a script létrehoz öt táblát, és a köztük levő kapcsolatokat is megcsinálja
	van benne egy down() fgv is, mely letörli a táblákat, a köztük levő kapcsolatokat és az indexelést

a rendszerünk ismeri a model verzióját, és az adatbázis verzióját, és tudja hogy az utolsó migrációs lépés (update-database)
után nem hiányzik semmi (nem volt módosítás)
Ezt a __MigrationHistory táblából tudja. 
A __MigrationHistory táblában a record migrationID-ja megegyezik a Migrations könyvtárban létrejött, fent említett nnnn_identity datamodel.cs
filenévben az nnnn -el
ezek alapján dolgozik az update-database

A modell módosítása után az add-migration paranccsal készülnek a módosító lépések

a down fgv-el visszafelé lépdelünk

PM>update-database -t 0
fenti paranccsal a legelejére megyünk vissza, egy üres adatbázist kapunk táblák nélkül (nincs meg a már legyártott öt táblánk)
a -t a TargerMigration paraméter rövidítése, a 0 pedig minden lépés előtti állapot 
a -Script paraméterrel nem fut le a módosítás, hanem megmutatja nekünk azt az SQL scriptet, amit a migration step-ből generál


## Hogyan tudunk adatokat írni az adatbázisba? (Egy új táblát teszünk az adatbázisba)

tárolni akarjuk az étel nevét, leírását és árát
az étlapot hívjuk Menu-nek
több étlap lehet

- készítsük el az adatmodellt

	a Models kvtárbn már megvan az IdentityModels.cs, amit az Identity csinált
	de most ue. kvtárban létrehozunk egy saját osztályt: MenuItem.cs

	megcsináljuk a MenuItem osztály elemeit (property-ként)
	- public int Id {get; set;}   //primary key - elsődleges kulcs, kötelező
		(a névkonvenció alapján, ha egy változó int és Id nevű, abból lesz a PK, hacsak nincs más kijelölve a [Key] annotációval)
	- public string Name {get; set;}
	- public string Description {get; set;}
	- (árat egyelőre nem írunk bele)
	
- Tudjuk, hogy az adatbázis elérést mindig egy DBContex-ből leszármaztatott osztály végzi.
	Itt többszörös leszármaztatás van, az IdentityModels.cs kvtban van egy konstruktor
	Láthatjuk, hogy ebben van megadva a "DefaultConnection" mint default adatbázisnév
	A konstruktor után csinálunk egy property-t:
	public DBSet<MenuItem> MenuItems {get; set;}