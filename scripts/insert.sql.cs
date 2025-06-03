INSERT INTO DeviceType (Name) VALUES
    ('PC'),
('Smartwatch'),
('Embedded'),
('Monitor'),
('Printer');

INSERT INTO Position (Name, MinExpYears) VALUES
    ('Software Engineer', 3),
('System Administrator', 2),
('Project Manager', 5),
('IT Support', 1),
('HR Manager', 4);

INSERT INTO Person (PassportNumber, FirstName, MiddleName, LastName, PhoneNumber, Email) VALUES
    ('AA123456', 'Anderson', 'Oral', 'Doe', '+1234567893', 'anderson.doe@example.com'),
('BB654321', 'Moti', NULL, 'Smith', '+1987654331', 'moti.smith@example.com'),
('CC987654', 'Lola', 'Marie', 'Ponla', '+1123456589', 'lola.marie@example.com'),
('DD246810', 'Bos', NULL, 'Black', '+1222777444', 'bos.black@example.com'),
('EE135791', 'Ese', 'Aja', 'Da', '+1339955777', 'ese.dajua@example.com');

INSERT INTO Device (Name, IsEnabled, AdditionalProperties, DeviceTypeId) VALUES
    ('HP Pro  ', 0, '{"operationSystem": null}', 1),
(' Loly Gen 5 Tiny', 1, '{"operationSystem": "Windows 11 Pro "}', 1),
('Apple Watch  2', 1, '{"battery": "50%"}', 2),
('Windo  4', 0, '{"ipAddress": "192.188.0.1", "network": "example"}', 3),
('Dell  Jp', 1, '{"ports": [{"type": "HDMI", "version": "2.1"}]}', 4),
('HP  Ultra', 0, '{colors: "black and white"}', 5);

INSERT INTO Employee (Salary, PositionId, PersonId, HireDate) VALUES
    (75000.00, 1, 1, '2021-03-01'),
(65000.00, 2, 2, '2022-05-15'),
(90000.00, 3, 3, '2020-02-20'),
(55000.00, 4, 4, '2023-09-10'),
(80000.00, 5, 5, '2019-11-01');

INSERT INTO DeviceEmployee (DeviceId, EmployeeId, IssueDate, ReturnDate) VALUES
    (1, 1, '2023-09-10', NULL),
(2, 2, '2023-01-20', '2024-01-15'),
(3, 3, '2022-01-05', NULL),
(4, 4, '2023-02-01', NULL),
(5, 5, '2021-05-25', '2023-05-25');