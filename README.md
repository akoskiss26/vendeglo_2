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


- Ahhoz h a model módosítása az adatbázisba kerüljön, először:
	PM> add-migration 'add MenuItem table'   

(Egyszerre csak egy migrációs lépésről lehet update-elni, tehát ha már van migrációs lépés, nem lehet egy újabb, hanem kell egy update-database)

- Ha megvan a migrációs lépés, jöhet az:
	PM> update-database 

-fentiek után (befrissítve az ObjectExpolrer-t) megjelenik a MenuItems, és van benne Id, Name, Description

tehát: Létrehozunk egy saját modelt és ezt úgy tudjuk bekötni az Identity adatbázisba, hogy a \Models\IdentityModels.cs-ben lévő 
ApplicationBDContent osztályt használjuk

11_Kezdő spec., beejelentkezés 11
---------------------------------
migrációs lépések visszavonása, újra lefuttatása az alábbi parancsokkal:
PM> update-database -t, 0
PM> update-database -t, 'id of migration step'




##12_Kezdő spec., beejelentkezés 12
---------------------------------
Inkrementális adatbázis modellezés a Code First Migration segítségével

## Módosítjuk az adatmodell-t, és bejátszuk az SQL szerverre 
(Árat hozzáadunk a táblához)

	- a Model-ben hozzátesszük az új property-t:
	public int Price {get; set;} 
	
	PM> add-migration 'add MenuItem.Price column'

	PM> update-database

összefoglalva:
--------------
módosítottuk az adatmodellt (hozzáadtunk egy oszlopot, vagy megváltoztattunk egy tulajdonságot)
Az add-migration 'name' paranccsal az adatmodellt kiírtuk egy migration step-be
ezzel létrejön a Migrations\20191128nnnn_name datamodel.cs és még két állomány
fenti egy adatbázis módosító lépés (migration step)
ebben van egy Up() és egy Down() fgv, ezek a módosítást bejátszák az adatbázisba, 
illetve kiveszik a már berakott módosítást az adatbázisból

utána a migration step-et kiírjuk az adatbázisba:
update-database paranccsal 
fontos, hogy a web.config-ban van megadva az adatelérési út:
a <ConnectionStrings> részben tudjuk megadni az adatbázis nevét, elérési útját és az autentikációs módszert

A __MigrationHistory táblában regisztrálja a módosító lépéseket, ennek alapján a lépések visszavonhatók és újra bejátszhatók 
az update-database -t 'name'  paranccsal. Itt a name a migrációs lépés neve, ami szerepel a megfelelő add-migration 'name'
parancsban, és a __MigrationHistory táblában
az update-database -t, 0 paranccsal a kezdethez megyünk, a teljes lépéssorozatot visszavonhatjuk
az update-database paranccsal az utolsó migration step-ig bejátszuk a módosításokat


##13_Étlap és adatbázis1 (2. nap)
---------------------------------

(eddigiek ismétlése és összefoglalása)

Db készítéshez a CF Migration-t használjuk
a user-ek kezeléséhez at ASP.NET Identity-t használjuk
	CF-el létrehozza a saját db-át
	most a db-t nem a DbContext-ből származtatjuk le, mint korábban,
 	hanem az IdentityDbContex-ből, hogy meglegyen az Identity adatbázisa


##14_Étlap és adatbázis2 (2. nap folyt.)
--------------------------------------
- csinálunk egy controllert
	controllers mappa jobb gomb -> Add -> Controler -> MVC5 Contr. w views, using Entity Framework
	Model Class: MenuItem
	Data context class: ApplicationDbContent 
	többi alapértelmezésben

generálódott a MenuItems nevű kontroller, amegfelelő action-ökkel, ez az IIS elindításával látható is
http://localhostnnn/MenuItem
mivel nem adtuk meg az action-t, az index-re fog vinni
itt a Create New -val új menüelemet tudunk létrehozni

most bárki tud új elemet felvinni, csináljuk meg, hogy csak a bejelentkezett user tudja ezt:
ASP.NET Identity csinálja

	- hogyan oldjuk meg, hogy csak bizonyos user-ek férjenek hozzá egy controller erőforrásaihoz:
	Controller annotálása az [Authorize] -al --> ezt a controllert csak bejelentkezett user használhatja
	az annotációt az osztálydefiníció fölé írjuk


##15_Étlap és adatbázis3 (2. nap folyt.)
--------------------------------------
 - szeretnénk, ha a be nem jelentkezett user az étlapot láthatná:
	- az azonoítatlan felhasználó (anonymous user) férjen hozzá a kontroller index és details action-jéhez
	- a Create, Edit, Delete pdig csak a bejelentkezett user-nek legyen megengedve
	- megoldás: az [Authorize] használható az action-ok előtt is
	- (egy action fgv elé több annotáció is írható)

- a kód tagolására jól használható a #region  .... #endregion kijelölés!


##16_Étlap és adatbázis4 (2. nap folyt.)
--------------------------------------

- web-es alkalmazásoknál a nem elérhető részeket nem jelenítjük meg:

	-az index oldalon (views/MenuItems/index) vannak olyan linkek, amiket a nem bejelentkezett user nem használ, 
	ezeket kellene nem megjeleníteni  --> a view-kban kellene a diszkussziót elvégezni
	- látjuk, hogy html.ActionLink-ek vannak az action-ökre
	- esetszétválasztással megállapítjuk, hogy a beeső kérés küldője be van-e jelentkezve:
		@if(Request.IsAuthenticated)
		   {
		    } 
	a @ -al jelezzük, hogy C# kód következik, és a fordító tudja h ez addig tart, amíg értelmes lezárá nincs, tehát pl. az if() {} végéig 
	a {}-be írjuk a meg/meg nem jelenítendő actionLink-et
	ennél a módszernél a {}-ben levő információ nem is megy ki a böngészőbe, tehát teljesen biztonságos

	A razor az alábbiak szerint értelmezi a kódot:
	Ha @ --> C# kód jön, egészen az "értelmes" lezárásig (kódblokk vége, ;, stb.), ha nincs zárójel, csak a közvetlen utána levő kifejezést 		értelmezi
	Ha <> (azaz HTML tag) --> html kód, egészen a vége tag-ig, vagy a következő @-ig
	A html és C# kódok egymásba ágyazhatók! 
	
	html kódú kiírásnál figyeljünk a div és a span közötti különbségre a mi a megjelenítésben van (pl. sortörés)



##17_Étlap és adatbázis5 (2. nap folyt.)
--------------------------------------

étlap link megjelenítése a főoldalon
	a cim meghívása után lefut a _ViewStart.cshtml, és lefut az abban lévő - most: 
	~/views/Shared/_Layout.cshtml  (ez mindig megjelenik)
	a _Layout.cshtml-ből látjuk, hogy bootstrap-et használ, a verziót  a Content/bootstrap.css-ben látjuk
	a _Layout.cshtml-ben megtaláljuk a navbar-t, abban pedig a menü elemeket ("név", "action", "controller")

bootstrap help:
www.getbootstrap.com
	ebben a navbar részleteit megtaláljuk

	a _Layout.cshtml-ben kitöröljük javítjuk és/vagy kitöröljük a menüelemeket
	a javítással a szendvics-menü is változik!


##18_Étlap és adatbázis 6 (2. nap folyt.)
--------------------------------------

az ételek szakaszokba rendezése
 (adatbázis és megjelenítési problémák lesznek)

szivárgó absztrakciók törvénye - Joel Spolsky [joelonsoftware.com]

 - lehetne, hogy felveszünk még egy property-t (oszlopot) a menuItem.cs-be, ami a szakaszokat (kategóriákat) tartalmazná,
    így minden item-hez hozzárendelhetnénk a kategórianevét, pl. előétel, főétel, desszert
 - EZ NEM JÓ (antipattern) mert ha át kell írni a deszertet desszert-re, minden item-en át keel írni

 - HELYETTE normalizálunk: csinálunk egy másik táblázatot (saját osztályt), aminek minden tétele megint tartalmaz Id-t: 1, 2, 3..stb
   az első táblázatban nem a leveseket, mit kategóriát írjuk, hanem csak az id-t
   így már két osztályunk van: MenuItems és Category



 
##19_Étlap és adatbázis 7 (2. nap folyt.)
--------------------------------------
 ételek szakaszokba rendezése folytatás

-azt akarjuk, hogy a két tábla legyen összekötve

	- a MenuItem osztályba csináljunk egy új propertyt:
	public Category Category {get; set;}

	- csináltassuk meg hozzá a Category class-t

	- a Category class-ba csináljunk kér propertyt:
	public int Id {get; set}
	public sring Name {get; set;}

	az Id elnevezés miatt az a tábla elsődleges kulcsa lesz, ami automatilusan emelkedik
	ez az Identity és a CodeFirst miatt van!
	ue. lenne, ha kiírnánk a [key] annotációt

tehát megcsináltuk az új osztályt, és a hivatkozást rá a MenuItem osztályban

ezt a módosítást ki kell írni az adatbázisba
	add-migration 'add Category class and MenuItem.Category column'

ha megnézzük, most a Category elemei lehetnek NULL
de minden elemenek kell tartozni valahová, mert másképp nem tudjuk megjeleníteni!
tehát jó, hogy megvan a Category, de ki kell egészíteni hogy
	- nem lehett NULL, 
	- webfelületet kell hozzá gyártani
	- ki kell tenni a MenuItem-re is