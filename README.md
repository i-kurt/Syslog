# Syslog
Zyxel modemler için syslog server.

Pandemi sürecinde uzaktan çalışma ve eğitim alma hayatımızın bir parçası oldu. Bu süreçte modemlere yapılan siber saldırılarda da artış olduğunu gördük.

Proje iki aşamadan oluşmaktadır.

1. Windows Service olarak kullanılabilen ve siber saldırılarda uyarı veren console uygulaması.

2. Mikroservis mimariye uygun olarak hazırlanmış, .Net Core ile yazılmış, docker desteği olan bir console uygulaması. Gelen atakları Postgresql veritabanına kaydetmektedir.
