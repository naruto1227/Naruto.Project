
DROP TABLE IF EXISTS `cap.received`;
CREATE TABLE `cap.received`  (
  `Id` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Name` varchar(400) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Group` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Content` longtext CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `Retries` int(11) NULL DEFAULT NULL,
  `Added` datetime(0) NOT NULL,
  `ExpiresAt` datetime(0) NULL DEFAULT NULL,
  `StatusName` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Version` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 221 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

SET FOREIGN_KEY_CHECKS = 1;
