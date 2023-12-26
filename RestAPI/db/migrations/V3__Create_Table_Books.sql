CREATE TABLE IF NOT EXISTS `books` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `title` varchar(80) NOT NULL,
  `author` varchar(80) NOT NULL,
  PRIMARY KEY (`id`)
) 