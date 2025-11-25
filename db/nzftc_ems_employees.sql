CREATE DATABASE  IF NOT EXISTS `nzftc_ems` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `nzftc_ems`;
-- MySQL dump 10.13  Distrib 8.0.44, for Win64 (x86_64)
--
-- Host: localhost    Database: nzftc_ems
-- ------------------------------------------------------
-- Server version	9.5.0

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
SET @MYSQLDUMP_TEMP_LOG_BIN = @@SESSION.SQL_LOG_BIN;
SET @@SESSION.SQL_LOG_BIN= 0;

--
-- GTID state at the beginning of the backup 
--

SET @@GLOBAL.GTID_PURGED=/*!80000 '+'*/ 'f05006c3-be06-11f0-9d04-d8bbc19dc3e3:1-1857';

--
-- Table structure for table `employees`
--

DROP TABLE IF EXISTS `employees`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `employees` (
  `EmployeeId` int NOT NULL AUTO_INCREMENT,
  `EmployeeCode` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `FirstName` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `LastName` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Birthday` datetime(6) DEFAULT NULL,
  `Gender` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Address` varchar(300) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Phone` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Email` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `PasswordHash` longblob,
  `PasswordSalt` longblob,
  `Department` varchar(80) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `PayFrequency` tinyint unsigned NOT NULL,
  `StartDate` datetime(6) NOT NULL,
  `JobPositionId` int DEFAULT NULL,
  `PayGradeId` int DEFAULT NULL,
  PRIMARY KEY (`EmployeeId`),
  KEY `IX_Employees_JobPositionId` (`JobPositionId`),
  KEY `IX_Employees_PayGradeId` (`PayGradeId`),
  CONSTRAINT `FK_Employees_jobpositions_JobPositionId` FOREIGN KEY (`JobPositionId`) REFERENCES `jobpositions` (`JobPositionId`),
  CONSTRAINT `FK_Employees_paygrades_PayGradeId` FOREIGN KEY (`PayGradeId`) REFERENCES `paygrades` (`PayGradeId`)
) ENGINE=InnoDB AUTO_INCREMENT=1005 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `employees`
--

LOCK TABLES `employees` WRITE;
/*!40000 ALTER TABLE `employees` DISABLE KEYS */;
INSERT INTO `employees` VALUES (1001,'NZFTC1001','Temp','Admin','1990-01-01 00:00:00.000000','Other','N/A',NULL,'admin@nzftc.local',_binary ']ù\Ú6·\Èv¯J\âo´¤V3\Û\n˜‘®ž^©¢Œ¿ö',_binary 'Ç„š’¦.°ñc\rB¬J\0W£\Î/|úO\è\î(3|\è¾üh*y\ïš+™™€\åV~H\Ê\ÉÚ¯¾8JŸ”Qu£4®ð','HR',0,'2025-11-20 00:00:00.000000',11,8),(1002,'NZFTC1002','TEMP','Emp','1995-05-15 00:00:00.000000','Male','123 Finance Street',NULL,'emp@nzftc.local',_binary '•$†Îµjv}E9\ç\Å\çRY>Q•wúL\ÄÀ®µŽ­\îY',_binary '¡FH“(\Îe…4>zýú*K]®üW\"\îøSŒ•\Õ\n\à³‰X\èõö\Û-pª\Þ&\ègÉ”™R!•?µv(>Ž—\é','Finance',0,'2025-11-20 00:00:00.000000',4,7),(1003,'NZFTC1003','Sarah','Williams','1997-03-12 00:00:00.000000','Female','42 Eden Terrace',NULL,'sarah@nzftc.local','','','IT',0,'2025-11-10 00:00:00.000000',22,6),(1004,'NZFTC1004','Michael','Brown','1988-09-14 00:00:00.000000','Male','19 Queen Street',NULL,'michael@nzftc.local','','','Finance',0,'2025-10-05 00:00:00.000000',3,8);
/*!40000 ALTER TABLE `employees` ENABLE KEYS */;
UNLOCK TABLES;
SET @@SESSION.SQL_LOG_BIN = @MYSQLDUMP_TEMP_LOG_BIN;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-11-25 23:12:01
