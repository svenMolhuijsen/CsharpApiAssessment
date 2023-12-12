CREATE TABLE Addresses (
    Id          INTEGER PRIMARY KEY AUTOINCREMENT,
    Street      TEXT    NOT NULL,
    HouseNumber TEXT    NOT NULL,
    Postcode    TEXT    NOT NULL,
    City        TEXT    NOT NULL,
    Country     TEXT    NOT NULL
);
