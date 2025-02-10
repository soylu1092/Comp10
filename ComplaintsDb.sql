CREATE DATABASE ComplaintsDb;
GO


USE ComplaintsDb;
GO


CREATE TABLE Complaints (
    Id INT PRIMARY KEY IDENTITY(1,1), 
    Title NVARCHAR(255) NOT NULL, 
    Description TEXT, 
    ImagePath NVARCHAR(255), 
    CreatedAt DATETIME DEFAULT GETDATE() 
);
go

CREATE TABLE Comments (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ComplaintId INT, 
    Content TEXT NOT NULL, 
    CreatedAt DATETIME DEFAULT GETDATE(), 
    CONSTRAINT FK_ComplaintId FOREIGN KEY (ComplaintId) REFERENCES Complaints(Id) 
);
GO

INSERT INTO Complaints (Title, Description, ImagePath, CreatedAt)
VALUES 
('Kötü Müşteri Hizmetleri', 'Müşteri hizmetleri çok ilgisizdi ve sorunum çözülmedi.', NULL, '2025-02-10 08:30:00'),
('Yavaş Teslimat', 'Siparişim tam 5 gün geç teslim edildi.', 'images/delivery_issue.jpg', '2025-02-09 10:15:00'),
('Hatalı Ürün', 'Aldığım ürün bozuk çıktı, iade etmek istiyorum.', NULL, '2025-02-08 14:45:00');

INSERT INTO Comments (ComplaintId, Content, CreatedAt)
VALUES 
(1, 'Müşteri hizmetleriyle 3 kez konuştum ama bir çözüm bulamadım.', '2025-02-10 09:00:00'),
(1, 'Bence müşterilerin sorunlarına daha hızlı çözüm bulmalısınız.', '2025-02-10 09:05:00'),
(2, 'Yavaş teslimat gerçekten sinir bozucu. Daha hızlı olmalısınız.', '2025-02-09 11:00:00'),
(3, 'Ben de aynı sorunu yaşadım, ürünleri kontrol etmenizi tavsiye ederim.', '2025-02-08 15:00:00');


