-- MySQL dump 10.13  Distrib 8.0.16, for Win64 (x86_64)
--
-- Host: localhost    Database: bandaid
-- ------------------------------------------------------
-- Server version	8.0.16

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
 SET NAMES utf8 ;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `events`
--

DROP TABLE IF EXISTS `events`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `events` (
  `EventId` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(45) NOT NULL,
  `Date` datetime NOT NULL,
  `Adress` varchar(254) DEFAULT NULL,
  `PhoneNumber` varchar(45) DEFAULT NULL,
  `Description` longtext NOT NULL,
  `ImgUrl` varchar(254) DEFAULT NULL,
  `UserId` int(11) NOT NULL,
  `StatusId` int(11) NOT NULL,
  PRIMARY KEY (`EventId`),
  KEY `_idx` (`UserId`),
  KEY `FK_Event_Status_idx` (`StatusId`),
  CONSTRAINT `FK_Event_Status` FOREIGN KEY (`StatusId`) REFERENCES `statuses` (`StatusId`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_Event_User` FOREIGN KEY (`UserId`) REFERENCES `users` (`UserId`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=26 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `events`
--

LOCK TABLES `events` WRITE;
/*!40000 ALTER TABLE `events` DISABLE KEYS */;
INSERT INTO `events` VALUES (1,'Subota','2016-05-20 19:00:00','Zagrebacka 3',NULL,'test1',NULL,1,4),(2,'Nedjelja','2017-05-20 19:00:00','Varazdinska 20',NULL,'test2',NULL,1,5);
/*!40000 ALTER TABLE `events` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `reviews`
--

DROP TABLE IF EXISTS `reviews`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `reviews` (
  `ReviewId` int(11) NOT NULL AUTO_INCREMENT,
  `Rate` int(11) NOT NULL,
  `Date` datetime NOT NULL,
  `Description` longtext,
  `UserId` int(11) NOT NULL,
  PRIMARY KEY (`ReviewId`),
  KEY `FK_Review_User` (`UserId`),
  CONSTRAINT `FK_Review_User` FOREIGN KEY (`UserId`) REFERENCES `users` (`UserId`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `reviews`
--

LOCK TABLES `reviews` WRITE;
/*!40000 ALTER TABLE `reviews` DISABLE KEYS */;
/*!40000 ALTER TABLE `reviews` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `statuses`
--

DROP TABLE IF EXISTS `statuses`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `statuses` (
  `StatusId` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(45) NOT NULL,
  PRIMARY KEY (`StatusId`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `statuses`
--

LOCK TABLES `statuses` WRITE;
/*!40000 ALTER TABLE `statuses` DISABLE KEYS */;
INSERT INTO `statuses` VALUES (1,'Odobreno'),(2,'UTijeku'),(3,'Odbijeno'),(4,'Zavrseno'),(5,'Otvoreno'),(6,'Otkazano');
/*!40000 ALTER TABLE `statuses` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `userroles`
--

DROP TABLE IF EXISTS `userroles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `userroles` (
  `RoleId` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  PRIMARY KEY (`RoleId`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `userroles`
--

LOCK TABLES `userroles` WRITE;
/*!40000 ALTER TABLE `userroles` DISABLE KEYS */;
INSERT INTO `userroles` VALUES (1,'Admin'),(2,'Izvodac'),(3,'Organizator');
/*!40000 ALTER TABLE `userroles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `users` (
  `UserId` int(11) NOT NULL AUTO_INCREMENT,
  `Email` varchar(254) NOT NULL,
  `Name` varchar(50) NOT NULL,
  `PhoneNumber` varchar(50) DEFAULT NULL,
  `Street` varchar(50) DEFAULT NULL,
  `PostCode` varchar(45) DEFAULT NULL,
  `City` varchar(45) DEFAULT NULL,
  `PassHash` text NOT NULL,
  `Description` longtext,
  `ProfileImg` longtext,
  `RoleId` int(11) NOT NULL,
  `IsEmailVerified` int(11) NOT NULL,
  `ActivationCode` varchar(64) NOT NULL,
  `Youtube` mediumtext,
  `Instagram` mediumtext,
  `Facebook` mediumtext,
  PRIMARY KEY (`UserId`),
  UNIQUE KEY `Email_UNIQUE` (`Email`),
  KEY `FK_User_UserRole` (`RoleId`),
  CONSTRAINT `FK_User_UserRole` FOREIGN KEY (`RoleId`) REFERENCES `userroles` (`RoleId`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'admin@admin.admin','admin',NULL,NULL,NULL,NULL,'JAvlGPq9JyTdtvBO6x2llnRI1+gxwIyPqCKAn3THIKk=',NULL,NULL,1,0,'g2i2sZwfDvE+LkFpRfGBggVKcOgvdLwaLqFgAhxw+Ko=',NULL,NULL,NULL),(2,'robert@robert.robert','robert',NULL,NULL,NULL,NULL,'2dLnNse+f6fOANM7HUOWdDbEUNJWn5rNweiqK6/4cgQ=',NULL,NULL,2,0,'SWEZiyn/Z5JBDrrweiRDjbNfPNR3bjdv2NLeOMcDvvk=',NULL,NULL,NULL),(4,'robbie_horvat@hotmail.com','Robert',NULL,NULL,NULL,NULL,'p5BNQ97k2aKZzZRyn74Ekve6qRGHkyM/usQvVNTFElM=',NULL,NULL,2,0,'mVF3d7RbvpjdADhyls3az4T5kceQaVowUuYHZM+bRHM=',NULL,NULL,NULL),(5,'meme@meme.meme','meme','0947359483','dijwbadiwabdhbaw','124234','cacafesfve','4GbPBXWcyfyDpVjvxLe+ns7Q7oXExFFsgHvJaOKPsIE=',NULL,'~/ProfilePics/3fgzkmug9nx11.png',2,0,'7kJZ5pKR8ac7m5r1wIiZ06BrvVa70QtysW9WpzJETps=','www,youtube.com','www.instagram.com','www.facebook.com'),(6,'m@m.m','majmun',NULL,NULL,NULL,NULL,'6fQybERz8/b0Sp382JXAkima+Sw+TeeAZnju/JhUlTY=',NULL,NULL,3,0,'VD1DPmw4mZc3XaFZUyAwW9xbnARliUJCeSHM+EL3Xqs=',NULL,NULL,NULL);
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2019-09-06 22:50:12
