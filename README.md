# skolerute
For å ikke laste opp alle packages når hver av oss pusher, følg følgende steps:

0. Sikkerhetskopier arbeidet du har gjort

1. Lag en mappe (workspace)

2. Gå inn i terminalen og finn fram til workspacen din

3. Skriv "git init"

4. Skriv git remote add origin https://github.com/dat210-gruppe3/skolerute

5. Sjekk at følgende fetch og push finnes ved å skrive "git remote -v"
origin	https://github.com/dat210-gruppe3/skolerute (fetch)
origin	https://github.com/dat210-gruppe3/skolerute (push)

6. Hent vår repository ved å skrive "git pull origin master"

7. Gå inn i fram .git-mappa ved å skru på "Vis skjulte filer"-innstillingen.  Åpne info-mappa. Åpne exclude-fila i en teksteditor.
  
  Alternativt, kan følgende steps a-c følges istedenfor på Mac OSX. 

  a. Mens du nå er i din workspace i terminalen (sjekk ved "pwd"), gå til .git-mappa di ved å skrive "cd .git"

  b. Bla videre til info-mappen (cd info)

  c. Åpne exlude-fila som ligger inne i info-mappen i en teksteditor (på Mac brukte jeg kommandoen "open -a TextEdit exlclude")

8. I web-browseren din, gå til https://github.com/dat210-gruppe3/skolerute/blob/master/gitignore og kopier alt som står der (RAW -> Ctrl-A -> Ctrl-C)

9. Lim inn alt i teksteditoren din som har åpnet exclude-fila. Lagre og lukk



Nå skal vi sjekke om alt fungerer. Følg videre følgende steps:

1. Hopp tilbake til workspacen din i terminalen

2. Test at alt fungerer ved å committe en liten forandring.
"git add ."

3. Sjekk at det vi har gjort fungerer ved å se på hva som skal bli committet. Skriv "git status".
Det har skjedd noe feil dersom det er mange filer som vises nå. KONTAKT MAGNUS

4. Nå kan vi commite ved å skrive "git commit -m "<din melding>""

5. Last opp ved å skrive "git push origin master"



Implementer arbeidet ditt fra sikkerhetskopien du gjorde i step 0. Lim inn det du har gjort fra sikkerhetskopien og inn i filene som (mest sannsynlig) allerede eksisterer i workspacen.



