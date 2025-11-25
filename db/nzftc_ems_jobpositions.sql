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
-- Table structure for table `jobpositions`
--

DROP TABLE IF EXISTS `jobpositions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `jobpositions` (
  `JobPositionId` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Department` varchar(80) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `PayGradeId` int NOT NULL,
  `AccessRole` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Description` varchar(400) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `IsActive` tinyint(1) NOT NULL,
  PRIMARY KEY (`JobPositionId`),
  UNIQUE KEY `IX_jobpositions_Name` (`Name`),
  KEY `IX_jobpositions_PayGradeId` (`PayGradeId`),
  CONSTRAINT `FK_jobpositions_paygrades_PayGradeId` FOREIGN KEY (`PayGradeId`) REFERENCES `paygrades` (`PayGradeId`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=34 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `jobpositions`
--

LOCK TABLES `jobpositions` WRITE;
/*!40000 ALTER TABLE `jobpositions` DISABLE KEYS */;
INSERT INTO `jobpositions` VALUES (1,'Chief Financial Officer (CFO)','Finance',10,'Admin',NULL,1),(2,'Finance Manager','Finance',9,'Admin',NULL,1),(3,'Senior Accountant','Finance',8,'Employee',NULL,1),(4,'Accountant','Finance',7,'Employee',NULL,1),(5,'Accounts Payable Officer','Finance',3,'Employee',NULL,1),(6,'Accounts Receivable Officer','Finance',3,'Employee',NULL,1),(7,'Payroll Officer','Finance',6,'Employee',NULL,1),(8,'Finance Administrator','Finance',2,'Employee',NULL,1),(9,'Billing Specialist','Finance',2,'Employee',NULL,1),(10,'Accounts Assistant','Finance',1,'Employee',NULL,1),(11,'HR Manager','HR',8,'Admin',NULL,1),(12,'Senior HR Advisor','HR',7,'Admin',NULL,1),(13,'HR Advisor','HR',3,'Admin',NULL,1),(14,'HR Coordinator','HR',2,'Admin',NULL,1),(15,'HR Administrator','HR',1,'Admin',NULL,1),(16,'Recruitment Specialist','HR',6,'Admin',NULL,1),(17,'Talent Acquisition Coordinator','HR',3,'Admin',NULL,1),(18,'Training & Development Officer','HR',6,'Admin',NULL,1),(19,'IT Manager','IT',8,'Admin',NULL,1),(20,'Systems Administrator','IT',3,'Employee',NULL,1),(21,'Network Administrator','IT',6,'Employee',NULL,1),(22,'Software Developer','IT',6,'Employee',NULL,1),(23,'Application Support Analyst','IT',3,'Employee',NULL,1),(24,'IT Support Technician','IT',2,'Employee',NULL,1),(25,'Helpdesk Support','IT',1,'Employee',NULL,1),(26,'Database Administrator (DBA)','IT',6,'Employee',NULL,1),(27,'Operations Manager','Operations',8,'Admin',NULL,1),(28,'Team Leader – Operations','Operations',4,'Employee',NULL,1),(29,'Supervisor – Operations','Operations',5,'Employee',NULL,1),(30,'Senior Officer – Operations','Operations',3,'Employee',NULL,1),(31,'Office Administrator','Operations',2,'Employee',NULL,1),(32,'Customer Service Representative','Operations',2,'Employee',NULL,1),(33,'Data Entry Operator','Operations',1,'Employee',NULL,1);
/*!40000 ALTER TABLE `jobpositions` ENABLE KEYS */;
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

-- Dump completed on 2025-11-25 23:12:02
