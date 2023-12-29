CREATE TABLE `claims` (
    `Key` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Value` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `UserId` INT(11) NOT NULL,
    CONSTRAINT `PK_claims` PRIMARY KEY (`UserId`, `Key`, `Value`),
    CONSTRAINT `FK_claims_users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`id`) ON DELETE CASCADE);