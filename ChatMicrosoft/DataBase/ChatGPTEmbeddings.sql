-- Host: localhost    Database: embeddings
-- ------------------------------------------------------
-- Server version	8.0.17
CREATE DATABASE IF NOT EXISTS `embeddings`;
USE `embeddings`;

DROP TABLE IF EXISTS `file_embeddings`;

CREATE TABLE `file_embeddings` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `file_name` varchar(255) NOT NULL,
  `text_file` text NOT NULL,
  `upload_date` datetime NOT NULL,
  `file_embedding` json,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;