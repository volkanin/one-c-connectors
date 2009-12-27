<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
    <xsl:output method="xml"/>
    <xsl:param name="contrId"/>

    <xsl:template match="/">
        <order-request>
            <contr-id><xsl:value-of select="$contrId"/></contr-id>
            <xsl:copy-of select="//shipment-date"/>
            <xsl:copy-of select="//order-id"/>
            <xsl:copy-of select="//price-overhead"/>
            <xsl:copy-of select="//positions"/>
        </order-request>
    </xsl:template>

</xsl:stylesheet>
