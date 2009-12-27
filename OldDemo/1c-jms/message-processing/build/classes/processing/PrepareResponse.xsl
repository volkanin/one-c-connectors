<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
    <xsl:output method="xml"/>
    <xsl:param name="contrId"/>

    <xsl:template match="/">
        <order-response>
            <xsl:copy-of select="//order-id"/>
            <xsl:copy-of select="//status"/>
            <xsl:copy-of select="//comment"/>
            <xsl:copy-of select="//diff-positions"/>            
        </order-response>
    </xsl:template>

</xsl:stylesheet>
