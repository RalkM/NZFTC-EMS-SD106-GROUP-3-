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
-- Table structure for table `employeepayrollsummaries`
--

DROP TABLE IF EXISTS `employeepayrollsummaries`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `employeepayrollsummaries` (
  `EmployeePayrollSummaryId` int NOT NULL AUTO_INCREMENT,
  `PayrollPeriodId` int DEFAULT NULL,
  `PayrollRunId` int DEFAULT NULL,
  `EmployeeId` int NOT NULL,
  `PayRate` decimal(65,30) NOT NULL,
  `RateType` tinyint unsigned NOT NULL,
  `GrossPay` decimal(65,30) NOT NULL,
  `PAYE` decimal(65,30) NOT NULL,
  `KiwiSaverEmployee` decimal(65,30) NOT NULL,
  `KiwiSaverEmployer` decimal(65,30) NOT NULL,
  `ACCLevy` decimal(65,30) NOT NULL,
  `StudentLoan` decimal(65,30) NOT NULL,
  `Deductions` decimal(65,30) NOT NULL,
  `TotalHours` decimal(65,30) NOT NULL,
  `NetPay` decimal(65,30) NOT NULL,
  `Status` tinyint unsigned NOT NULL,
  `GeneratedAt` datetime(6) NOT NULL,
  `PaidAt` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`EmployeePayrollSummaryId`),
  KEY `IX_EmployeePayrollSummaries_EmployeeId` (`EmployeeId`),
  KEY `IX_EmployeePayrollSummaries_PayrollPeriodId` (`PayrollPeriodId`),
  KEY `IX_EmployeePayrollSummaries_PayrollRunId` (`PayrollRunId`),
  CONSTRAINT `FK_EmployeePayrollSummaries_Employees_EmployeeId` FOREIGN KEY (`EmployeeId`) REFERENCES `employees` (`EmployeeId`) ON DELETE CASCADE,
  CONSTRAINT `FK_EmployeePayrollSummaries_payrollperiods_PayrollPeriodId` FOREIGN KEY (`PayrollPeriodId`) REFERENCES `payrollperiods` (`PayrollPeriodId`),
  CONSTRAINT `FK_EmployeePayrollSummaries_payrollruns_PayrollRunId` FOREIGN KEY (`PayrollRunId`) REFERENCES `payrollruns` (`PayrollRunId`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `employeepayrollsummaries`
--

LOCK TABLES `employeepayrollsummaries` WRITE;
/*!40000 ALTER TABLE `employeepayrollsummaries` DISABLE KEYS */;
INSERT INTO `employeepayrollsummaries` VALUES (1,1,1,1001,50.000000000000000000000000000000,0,2000.000000000000000000000000000000,200.000000000000000000000000000000,45.000000000000000000000000000000,45.000000000000000000000000000000,15.000000000000000000000000000000,0.000000000000000000000000000000,260.000000000000000000000000000000,40.000000000000000000000000000000,1740.000000000000000000000000000000,2,'2025-11-13 00:00:00.000000',NULL);
/*!40000 ALTER TABLE `employeepayrollsummaries` ENABLE KEYS */;
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

-- Dump completed on 2025-11-25 23:12:00
