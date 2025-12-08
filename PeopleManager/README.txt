הוראות הפעלה ותיעוד קצר

1) פתיחה והרצה
   - פתחו את ה-Solution ב-Visual Studio (גרסה התומכת .NET Framework 4.8).
   - הריצו את הפרויקט (F5) כדי לפתוח את היישום ב-IIS Express.

2) מסלולים שימושיים
   - הוספת אדם: /People/Create
   - רשימת אנשים: /People/Index
   - ייצוא PDF: לחצן "ייצא כקובץ PDF" בעמוד הרשימה

3) תמונות
   - תמונות שנבחרות נשמרות גם כקובץ ב-`~/Uploads` וגם בזיכרון ה-DB (`PhotoData`, `PhotoContentType`).
   - תיקיית `Uploads` הוכנסה ל-.gitignore ואין לכלול אותה ב-Git.

4) מסד נתונים
   - בעת שינוי במודל, הוגדר Initializer שמוחק ומחדש את DB (DropCreateDatabaseIfModelChanges). נתונים יאבדו אם המודל משתנה.

5) בדיקות יחידה
   - הוסף פרויקט בדיקות `PeopleManager.Tests` (MSTest). להריץ בדיקות: בתפריט Test > Run All (Visual Studio Test Explorer).
   - ניתן גם להריץ באמצעות כלי הבדיקה המתאים (vstest.console) אם מותקן.

6) הערות נוספות
   - ישנה תמיכה בייצוא PDF הכוללת תמונות וטקסט עברי (נדרשת גופן ב-`App_Data/fonts`).
   - חלקים בקוד נוצרו או נערכו בעזרת כלי עזרה אוטומטיים.

